using TplWorkflow.Exceptions;
using TplWorkflow.Extensions.Validations;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace TplWorkflow.Extensions.Mappers
{
  public static class InputMapperExtension
  {
    private readonly static string DefaultType = typeof(IDictionary<string,object>).AssemblyQualifiedName;
    public static IList<Input> Map(this IList<InputTemplate> templates)
    {
     
      var inputs = new List<Input>();
      foreach(var template in templates)
      {
        inputs.Add(template.Map());
      }
      return inputs;
    }
    public static Input Map(this InputTemplate template)
    {
      EnrichInputTemplate(template);
      template.Kind.Required("Input should have kind.");

      //hot fix - backward compatibility 
      if (template.Parameters != null && template.Parameters.Count > 0)
      {
        template.Inputs = template.Parameters;
      }
      if (template.OfKind<InlineInput>())
      {
        return template.MapInlineInput();
      }
      if (template.OfKind<StepInput>())
      {
        return template.MapStepInput();
      }
      if (template.OfKind<VariableInput>())
      {
        return template.MapVariableInput();
      }
      if (template.OfKind<ExpressionInput>())
      {
        return template.MapExpressionInput();
      }
      throw new WorkflowException($"Input of kind {template.Kind} is not supported");
    }

    public static void EnrichInputTemplate(InputTemplate template)
    {
      if (string.IsNullOrEmpty(template.DataType))
      {
        template.DataType = DefaultType;
      } 
    }
    public static Input MapInlineInput(this InputTemplate template)
    {
      var type = Type.GetType(template.DataType);
      object data;
      try
      {
        data = ChangeType(template.Data, type);
      }
      catch(Exception ex) when (ex is InvalidCastException || ex is FormatException)
      {
        throw new WorkflowException("Inline input data and data type is not same.", ex);
      }
      return new InlineInput(type, data);
    }
    public static Input MapExpressionInput(this InputTemplate template)
    {
      template.Inputs.Required("Expression input must have expression inputs.");
      template.Expression.Required("Expression input must have a expression define.");
      template.DataType.Required("Expression input must have a data type define.");
      var inputs = new List<Input>();
      var peList = new List<ParameterExpression>();   
      foreach (var input in template.Inputs)
      {
        var _input = input.Map();
        var pe = Expression.Parameter(_input.DataType, input.Name);
        peList.Add(pe);
        inputs.Add(_input);
      }
      var destType = Type.GetType(template.DataType);
      var le = DynamicExpressionParser.ParseLambda(peList.ToArray(), destType, template.Expression);
      var method = le.Compile();
      return new ExpressionInput(destType, method, inputs);
    }

    public static Input MapStepInput(this InputTemplate template)
    {
      template.DataType.Required("Step Input shoud have datatype.");
      var type = Type.GetType(template.DataType);
      return new StepInput(type);
    }
    public static Input MapVariableInput(this InputTemplate template)
    {
      template.Name.Required("Variable Input shoud have a name define.");
      template.DataType.Required("Step Input shoud have datatype.");
      var type = Type.GetType(template.DataType);
      var scopeResolver = template.GetScopeResolver();
      return new VariableInput(template.Name, type, scopeResolver);
    }
    public static Type[] GetParameterTypes(this IList<InputTemplate> inputs, Func<InputTemplate, bool> predicate = null)
    {
      if (predicate != null)
      {
        return inputs.Where(predicate).Select(e => Type.GetType(e.DataType)).ToArray();
      }
      return inputs.Select(e => Type.GetType(e.DataType)).ToArray();
    }
    private static object ChangeType(object input, Type type)
    {
      if (input is IConvertible)
      {
        return Convert.ChangeType(input, type);
      }
      throw new WorkflowException($"Unable to case inline input data type for {type.Name}.");
    }
  }
}
