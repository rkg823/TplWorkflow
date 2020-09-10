using TplWorkflow.Core;
using TplWorkflow.Exceptions;
using TplWorkflow.Extensions.Validations;
using TplWorkflow.Models.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TplWorkflow.Extensions.Mappers
{
  public static class VariableMapperExtension
  {
    private static Type StringType = typeof(string);
    [Obsolete("This method is replaced with new method")]
    public static IList<Variable> Map(this IList<InputTemplate> variables, TemplateContext context)
    {
      var list = new List<Variable>();
      if (variables == null) return list;
      foreach (var variable in variables)
      {
        variable.Name.Required("Variable should have a name.");
        variable.Data.Required("Variable should have data.");
        variable.DataType.Required("Variable should have a data type.");
        var input = variable.MapInlineInput();
        var data = input.Resolve(default);
        if (list.Any(e => e.Name == variable.Name))
          throw new WorkflowException($"Duplicate variable with name {variable.Name}");
        list.Add(new Variable(variable.Name, data));
      }
      return list;
    }

    public static IList<Variable> Map(this IDictionary<string, object> variables)
    {      
      var _variables = new List<Variable>();
      if (variables == null) return _variables;
      foreach (var variable in variables)
      {
        if (variable.Value.IsJObject())
        {
          (string datatype, string data) = variable.Value.MapJObjectVariable();

          if (data != null && datatype != null)
          {
            var type = Type.GetType(datatype);
            _variables.Add(variable.Key, type, data);
            continue;
          } 
          _variables.Add(variable.Key, typeof(IDictionary<string, object>), variable.Value.ToString());
          continue;
        }

        var _type = variable.Value.GetType();
        _variables.Add(new Variable(variable.Key, variable.Value.ChnageType(_type)));
      }
      return _variables;

    }

    public static bool IsJObject<T>(this T item)
    {
      return item.GetType() == typeof(JObject);
    }

    public static void Add(this IList<Variable> variables,string key, Type type, string data)
    {
      if(type == StringType)
      {
        variables.Add(new Variable(key, data));
        return;
      }
      var instance = JsonConvert.DeserializeObject(data, type);
      variables.Add(new Variable(key, instance));
    }

    public static (string, string) MapJObjectVariable<T>(this T item)
    {
      var jObject = JObject.FromObject(item);
      var datatype = jObject.GetValue("datatype", StringComparison.OrdinalIgnoreCase)?.Value<string>();
      var json = jObject.GetValue("data", StringComparison.OrdinalIgnoreCase)?.ToString();
      return (datatype, json);
    }
  }
}
