// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Extensions.Mappers
{
  using TplWorkflow.Exceptions;
  using TplWorkflow.Extensions.Validations;
  using TplWorkflow.Core.Conditions;
  using TplWorkflow.Core.Pipelines;
  using TplWorkflow.Core.Steps;
  using TplWorkflow.Models.Templates;
  using System;
  using System.Collections.Generic;
  using TplWorkflow.Core;
  using TplWorkflow.Core.Maps;
  using TplWorkflow.Core.Inputs;
  using System.Linq;

  public static class PipelineMapperExtension
  {
    public static Pipeline Map(this PipelineTemplate template, TemplateContext context)
    {
      template.Kind.Required("Pipeline should have kind.");

      template = template.MapLinkPipeline(context);

      if (template.OfKind<IfPipeline>())
      {
        return template.MapIfPipeline(context);
      }

      template.Steps.Required("Pipeline should have steps.");

      if (template.OfKind<ParallelPipeline>())
      {
        return template.Map(context, (condition, steps, vars, maps, _) =>
        new ParallelPipeline(condition, steps, vars, maps));
      }

      if (template.OfKind<SequentialPipeline>())
      {
        return template.Map(context, (condition, steps, vars, maps, _) =>
        new SequentialPipeline(condition, steps, vars, maps));
      }

      if (template.OfKind<ForEachPipeline>())
      {
        return template.Map(context, (condition, steps, vars, maps, source) =>
        new ForEachPipeline(condition, steps, vars, maps, source));
      }

      if (template.OfKind<ParallelForeachPipeline>())
      {
        return template.Map(context, (condition, steps, vars, maps, source) =>
        new ParallelForeachPipeline(condition, steps, vars, maps, source, template.Dop ?? Environment.ProcessorCount));
      }

      throw new WorkflowException($"Pipeline of kind {template.Kind} not supported");
    }

    public static U Map<U>(this PipelineTemplate template, TemplateContext context, Func<Condition, IList<Step>, IList<Variable>, IList<Map>, Input, U> predicate)
    {
      var condition = template.Condition?.Map(context);
      var steps = template.Steps.Map(context);

      var maps = template.MapPipelineMappers();
      var source = template.MapForeachSource();
      var variables = template.MapPipelineVariables();

      return predicate(condition, steps, variables, maps, source);
    }

    public static Pipeline MapIfPipeline(this PipelineTemplate template, TemplateContext context)
    {
      template.Condition.Required("If pipeline should deifne condition.");
      template.When.Required("If pipeline should deifne when step.");

      var condition = template.Condition?.Map(context);
      var maps = template.MapPipelineMappers();
      var variables = template.MapPipelineVariables();

      var trueStep = template.When.FirstOrDefault(e => e.Key.ToLowerInvariant() == true.ToString().ToLowerInvariant()).Value.Map(context);
      var falseStep = template.When.FirstOrDefault(e => e.Key.ToLowerInvariant() == false.ToString().ToLowerInvariant()).Value.Map(context);

      return new IfPipeline(condition, variables, maps, template.Condition.Disable, trueStep, falseStep);
    }

    //public static Pipeline MapSwitchPipeline(this PipelineTemplate template, TemplateContext context)
    //{
    //  template.Condition.Required("If pipeline should deifne condition.");
    //  template.When.Required("If pipeline should deifne when step.");
    //  var condition = template.Condition?.Map(context);
    //  var maps = template.MapPipelineMappers();
    //  var variables = template.MapPipelineVariables();
    //  var keyvalues = new Dictionary<string, Step>();

    //  foreach(var kv in template.When)
    //  {
    //    keyvalues.Add(kv.Key, kv.Value.Map(context));
    //  }

    //  return new SwitchPipeline(condition, variables, maps, keyvalues);
    //}

    public static Input MapForeachSource(this PipelineTemplate template)
    {
      Input source = default;

      if (template.OfForeachKind())
      {
        template.Source.Required("foreach pipeline should deifne source.");

        if (template.Source.OfKind<InlineInput>())
        {
          throw new WorkflowException("Inline kind not supported for foreach");
        }
        
        template.Source.DataType = typeof(object).AssemblyQualifiedName;
        source = template.Source.Map();
      }

      return source;
    }

    public static IList<Map> MapPipelineMappers(this PipelineTemplate template)
    {
      var maps = new List<Map>();

      if (template.HasMaps())
      {
        maps.AddRange(template.Maps.Map());
      }

      return maps;
    }

    public static IList<Variable> MapPipelineVariables(this PipelineTemplate template)
    {
      var variables = new List<Variable>();

      if (!template.HasVariables())
      {
        return variables;
      }
       
      return template.Variables.Map();
    }
  }
}
