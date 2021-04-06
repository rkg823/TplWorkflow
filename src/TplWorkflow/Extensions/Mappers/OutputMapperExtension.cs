// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Extensions.Mappers
{
  using System.Collections.Generic;
  using System.Linq.Dynamic.Core;
  using System.Linq.Expressions;
  using TplWorkflow.Exceptions;
  using TplWorkflow.Extensions.Validations;
  using TplWorkflow.Core.Inputs;
  using TplWorkflow.Core.Outputs;
  using TplWorkflow.Models.Templates;

  public static class OutputMapperExtension
  {
    public static IList<Output> Map(this IList<OutputTemplate> templates)
    {
      var inputs = new List<Output>();

      foreach (var template in templates)
      {
        inputs.Add(template.Map());
      }

      return inputs;
    }

    public static Output Map(this OutputTemplate template)
    {
      template.Kind.Required("Output should have kind.");

      // hot fix - backward compatibility
      if (template.Parameters != null && template.Parameters.Count > 0)
      {
        template.Inputs = template.Parameters;
      }

      if (template.OfKind<StepOutput>())
      {
        return template.MapStepOutput();
      }

      if (template.OfKind<ExpressionOutput>())
      {
        return template.MapExpressionOutput();
      }

      throw new WorkflowException($"Output of kind {template.Kind} is not supported");
    }

    public static Output MapStepOutput(this OutputTemplate template)
    {
      template.Name.Required("Step output should have name.");

      var scopeResolver = template.GetScopeResolver();

      return new StepOutput(template.Name, scopeResolver);
    }
    public static Output MapExpressionOutput(this OutputTemplate template)
    {
      template.Inputs.Required("Expression output should have inputs.");
      template.Name.Required("Expression output should have a name.");
      template.Expression.Required("Expression output should have a expression define.");

      var inputs = new List<Input>();
      var peList = new List<ParameterExpression>();

      foreach (var input in template.Inputs)
      {
        var _input = input.Map();
        var pe = Expression.Parameter(_input.DataType, input.Name);

        peList.Add(pe);
        inputs.Add(_input);
      }
      var destType = typeof(object);
      var le = DynamicExpressionParser.ParseLambda(peList.ToArray(), destType, template.Expression);
      var method = le.Compile();
      var scopeResolver = template.GetScopeResolver();

      return new ExpressionOutput(template.Name, method, inputs, scopeResolver);
    }
  }
}
