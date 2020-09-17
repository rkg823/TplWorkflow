// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Inputs;
using TplWorkflow.Core.Methods;
using TplWorkflow.Core.Steps;
using TplWorkflow.Exceptions;
using TplWorkflow.Extensions.Validations;
using TplWorkflow.Models;
using TplWorkflow.Models.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TplWorkflow.Extensions.Mappers
{

  public static class StepMapperExtension
  {
  
    public static IList<Step> Map(this IList<StepTemplate> templates, TemplateContext context)
    {
      var steps = new List<Step>();
      foreach (var template in templates)
      {
        steps.Add(template.Map(context));
      }
      return steps;
    }
    public static Step Map(this StepTemplate template, TemplateContext context)
    {
      template.Kind.Required("Step should have kind.");
      if (template.OfKind<AsyncStep>())
      {
        return template.MapTaskStep(context);
      }
      if (template.OfKind<PipelineStep>())
      {
        return template.MapPipelineStep(context);
      }
      if (template.OfKind<WorkflowStep>())
      {
        return template.MapWorkflowStep(context);
      }
      if (template.OfKind<ExpressionStep>())
      {
        return template.MapExpressionStep(context);
      }
      if (template.OfKind<MacroStep>())
      {
        return template.MapMacroStep(context);
      }
      throw new WorkflowException($"Step of kind ${template.Kind} is not supported");
    }
    public static Step MapPipelineStep(this StepTemplate template, TemplateContext context)
    {
      var id = template.Id ?? Guid.NewGuid().ToString();

      var pipeline = template.Pipeline.Map(context);
      var condition = template.Condition?.Map(context);
      return new PipelineStep(id, pipeline, condition);
    }

    public static Step MapMacroStep(this StepTemplate template, TemplateContext context)
    {
      template.Inputs.Required("Macro step should have inputs.");
      template.Template.Required("Macro step should have expression.");
      var id = template.Id ?? Guid.NewGuid().ToString();
      var condition = template.Condition?.Map(context);
      var inputs = new List<Input>();
      var peList = new List<ParameterExpression>();
      foreach (var i in template.Inputs)
      {
        var _input = i.Map();
        var pe = Expression.Parameter(_input.DataType, i.Name);
        inputs.Add(_input);
        peList.Add(pe);
      }
     
      var exp = Regex.Unescape(template.Template.ToString());
      var pd = new Dictionary<string, (IList<Input> inputs, Delegate method)>();
      var matched = Regex.Matches(exp, @"{(.+?)}");
       for (int count = 0; count < matched.Count; count++)
      {
        var method = DynamicExpressionParser.ParseLambda(peList.ToArray(), null, matched[count].Groups[1].Value).Compile();
        pd.Add(matched[count].Groups[0].Value, (inputs, method));
      }
      return new MacroStep(id, exp, condition, pd);
    }

    public static Step MapExpressionStep(this StepTemplate template, TemplateContext context)
    {
      template.Inputs.Required("Expression step should have inputs.");
      template.Expression.Required("Expression step should have expression.");
      var id = template.Id ?? Guid.NewGuid().ToString();
      var condition = template.Condition?.Map(context);
      var inputs = new List<Input>();
      var peList = new List<ParameterExpression>();
      foreach (var temp in template.Inputs)
      {
        var _input = temp.Map();
        var pe = Expression.Parameter(_input.DataType, temp.Name);
        peList.Add(pe);
        inputs.Add(_input);
      }
      var method = DynamicExpressionParser.ParseLambda(peList.ToArray(), null, template.Expression).Compile();
      return new ExpressionStep(id, condition, method, inputs);
    }

    public static Step MapWorkflowStep(this StepTemplate template, TemplateContext context)
    {
      var id = template.Id ?? Guid.NewGuid().ToString();
      var condition = template.Condition?.Map(context);
      var version = template.Version ?? 1;
      return new WorkflowStep(id, template.Name, version, condition);
    }

    public static Step MapTaskStep(this StepTemplate template, TemplateContext context)
    {
      template.RequiredMethod();
      var id = template.Id ?? Guid.NewGuid().ToString();
      var inputs = template.Inputs?.Map();
      var outputs = template.Outputs?.Map();
      var condition = template.Condition?.Map(context);
      if (template.Contract != null)
      {
        var methodInfo = template.MapMethodInfo(template.Contract);
        var contractType = Type.GetType(template.Contract);
        var getTask = MapInstanceGetTask(methodInfo);
        var method = new ContractMethod(methodInfo,contractType,inputs, getTask);
        return new AsyncStep(id, method, outputs, condition);
      }

      if (template.Instance != null)
      {
        var methodInfo = template.MapMethodInfo(template.Instance);
        var instanceType = Type.GetType(template.Instance);
        var instance = Activator.CreateInstance(instanceType);
        var getTask = MapInstanceGetTask(methodInfo);
        var method = new InstanceMethod(methodInfo, instance, inputs, getTask);
        return new AsyncStep(id, method, outputs, condition);
      }

      if (template.Static != null)
      {
        var methodInfo = template.MapMethodInfo(template.Static);
        var getTask = MapStaticGetTask(methodInfo);
        var method = new StaticMethod(methodInfo, inputs, getTask);
        return new AsyncStep(id, method, outputs, condition);
      }

      throw new WorkflowException("Step must define Instance, Contract or Static type");
    }
    public static MethodInfo MapMethodInfo(this StepTemplate template, string source)
    {
      Type instanceType = Type.GetType(source);
      var paramTypes = template.Inputs.GetParameterTypes();
      var genericParamTypes = template.Inputs.GetParameterTypes(e => e.Generic);
      var nonGenericParamTypes = template.Inputs.GetParameterTypes(e => !e.Generic);
      if (genericParamTypes != null && genericParamTypes.Any())
      {
        return SearchAndGetGenericMethod(instanceType,template.Method, genericParamTypes, nonGenericParamTypes);
      }
      else
      {
        return SearchAndGetMethod(instanceType,template.Method, paramTypes);
      }
    }

    public static MethodInfo SearchAndGetMethod(Type type, string methodName, Type[] _params)
    {
      var _method = type.GetMethod(methodName, _params);
      if(_method != null)
      {
        return _method;
      }
      var _interfaces = type.GetInterfaces();
      if(_interfaces == null)
      { 
        return _method;
      }
      foreach(var _interface in _interfaces)
      {
        var _parentMethod = SearchAndGetMethod(_interface, methodName, _params);
        if(_parentMethod != null)
        {
          return _parentMethod;
        }
      }
      return _method;
    }

    public static MethodInfo SearchAndGetGenericMethod(Type type, string methodName, Type[] genericParamTypes, Type[] nonGenericParamTypes)
    {
      var _method = type.GetGenericMethod(methodName, genericParamTypes, nonGenericParamTypes);
      if (_method != null)
      {
        return _method;
      }
      var _interfaces = type.GetInterfaces();
      if (_interfaces == null)
      {
        return _method;
      }
      foreach (var _interface in _interfaces)
      {
        var _parentMethod = SearchAndGetGenericMethod(_interface, methodName, genericParamTypes, nonGenericParamTypes);
        if (_parentMethod != null)
        {
          return _parentMethod;
        }
      }
      return _method;
    }

    private static Func<MethodInfo, object, IList<object>, Task> MapInstanceGetTask(MethodInfo methodInfo)
    {
      if (methodInfo.IsTaskReturn())
      {
        return (MethodInfo _info, object _instance, IList<object> _inputs) =>
        {
          var tcs = new TaskCompletionSource<VoidReturn>();
          var method = _info.MakeDelegate(_instance);
          var _task = method.DynamicInvoke(_inputs.ToArray()) as Task;
          _task.ContinueWith((t) => tcs.SetResult(new VoidReturn()), TaskContinuationOptions.OnlyOnRanToCompletion);
          _task.ContinueWith((t) => tcs.SetException(t.Exception), 
            TaskContinuationOptions.OnlyOnFaulted);
          return tcs.Task;
        };
      }
      if (methodInfo.IsTaskOfTReturn())
      {
        return (MethodInfo _info, object _instance, IList<object> _inputs) =>
        {
          var method = _info.MakeDelegate(_instance);
          return method.DynamicInvoke(_inputs.ToArray()) as Task;
        };
      }
      if (methodInfo.IsVoidRetun())
      {
        return (MethodInfo _info, object _instance, IList<object> _inputs) =>
        {
          var method = _info.MakeDelegate(_instance);
          return Task.Run(() =>
          {
            method.DynamicInvoke(_inputs.ToArray());
            return new VoidReturn();
          });
        };
      }
      return (MethodInfo _info, object _instance, IList<object> _inputs) =>
      {
        var method = _info.MakeDelegate(_instance);
        return Task.Run(() =>
        {
          return method.DynamicInvoke(_inputs.ToArray());
        });
      };
    }

    private static Func<MethodInfo, IList<object>, Task> MapStaticGetTask(MethodInfo methodInfo)
    {
      if (methodInfo.IsTaskReturn())
      {
        return (MethodInfo _info, IList<object> _inputs) =>
        {
          var tcs = new TaskCompletionSource<VoidReturn>();
          var method = _info.MakeDelegate();
          var _task = method.DynamicInvoke(_inputs.ToArray()) as Task;
          _task.ContinueWith((t) => tcs.SetResult(new VoidReturn()), TaskContinuationOptions.OnlyOnRanToCompletion);
          _task.ContinueWith((t) => tcs.SetException(t.Exception),
            TaskContinuationOptions.OnlyOnFaulted);
          return tcs.Task;
        };
      }
      if (methodInfo.IsTaskOfTReturn())
      {
        return (MethodInfo _info, IList<object> _inputs) =>
        {
          var method = _info.MakeDelegate();
          return method.DynamicInvoke(_inputs.ToArray()) as Task;
        };
      }
      if (methodInfo.IsVoidRetun())
      {
        return (MethodInfo _info, IList<object> _inputs) =>
        {
          var method = _info.MakeDelegate();
          return Task.Run(() =>
          {
            method.DynamicInvoke(_inputs.ToArray());
            return new VoidReturn();
          });
        };
      }
      return (MethodInfo _info, IList<object> _inputs) =>
      {
        var method = _info.MakeDelegate();
        return Task.Run(() =>
        {
          return method.DynamicInvoke(_inputs.ToArray());
        });
      };
    }
  }
}
