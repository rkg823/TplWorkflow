// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Exceptions;
using TplWorkflow.Extensions.Validations;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Models.Templates;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Dynamic;
using TplWorkflow.Core;
using TplWorkflow.Stores;

namespace TplWorkflow.Extensions.Mappers
{
  public static class ConditionMapperExtension
  {
    private static string ObjectQualifiedName = typeof(object).AssemblyQualifiedName;
    public static Condition Map(this ConditionTemplate template, TemplateContext context)
    {
      //hot fix - backward compatibility 
      if (template.Parameters != null && template.Parameters.Count> 0)
      {
        template.Inputs = template.Parameters;
      }
      
      template.Kind.Required("Condition should have kind.");
      template = template.MapLinkCondition(context);
      if (template.OfKind<InlineCondition>())
      {
        return template.MapInlineCondition();
      }
      if (template.OfKind<ExpressionCondition>())
      {
        return template.MapExpressionCondition();
      }
      if (template.OfKind<StepCondition>())
      {
        return template.MapStepCondition(context);
      }
      if (template.OfKind<OrCondition>())
      {
        return template.MapLogicalCondition(context,
          (c)=> new OrCondition(c));
      }
      if (template.OfKind<AndCondition>())
      {
        return template.MapLogicalCondition(context,
         (c) => new AndCondition(c));
      }
      throw new WorkflowException($"Condition of kind {template.Kind} is not supported.");
    }

    public static Condition MapInlineCondition(this ConditionTemplate template)
    {
      return new InlineCondition(template.Data);
    }
    public static Condition MapExpressionCondition(this ConditionTemplate template)
    {  
      template.Expression.Required("Expression condtion should have a expression define");
      var inputs = new List<Input>();
      var peList = new List<ParameterExpression>(); 
      if(template.Inputs == null || template.Inputs.Count == 0)
      {
        template.Inputs = new List<InputTemplate>
        {
          new InputTemplate
          {
            Kind = DefinitionStore.StepInput,
            Name = "step",
            DataType = ObjectQualifiedName
          }
        };
      }
      foreach (var input in template.Inputs)
      {
        var _input = input.Map();
        var pe = Expression.Parameter(_input.DataType, input.Name);
        peList.Add(pe);
        inputs.Add(_input);
      }
      var destType = typeof(bool);
      var le = DynamicExpressionParser.ParseLambda(peList.ToArray(), destType, template.Expression);
      var method = le.Compile();
      var variables = template.Variables.Map();
      return new ExpressionCondition(method, inputs, variables);
    }

    public static Condition MapLogicalCondition(this ConditionTemplate template, TemplateContext context, Func<IList<Condition>, Condition> func)
    {
      var conditions = template.Conditions.Select(e => e.Map(context)).ToList();
      return func(conditions);
    }

    public static Condition MapStepCondition(this ConditionTemplate template, TemplateContext context)
    {
      var step = template.Step.Map(context);
      var _variables = template.Variables.Map();
      return new StepCondition(step, _variables);
    }
  }
}
