// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Extensions
{
  using TplWorkflow.Core;
  using TplWorkflow.Core.Common;
  using TplWorkflow.Core.Common.Interfaces;
  using TplWorkflow.Core.Inputs;
  using TplWorkflow.Core.Maps;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  public static class WorkflowExtensions
  {
    public static async Task<bool> ShouldRun(this IConditional pipeline, ExecutionContext context)
    {
      if (pipeline.Condition != null)
      {
        var result = await pipeline.Condition.Resolve(context);

        return await result.Value();
      }

      return true;
    }

    public static async Task<bool> ShouldSkip(this IConditional pipeline, ExecutionContext context)
    {
      return !await pipeline.ShouldRun(context);
    }

    public static async Task<IList<bool>> Evaluate(this IMultiCondition condition, ExecutionContext context)
    {
      var tasks = new List<Task<AsyncResult<bool>>>();
      
      foreach (var _condition in condition.Conditions)
      {
        var result =  _condition.Resolve(context);
        tasks.Add(result);
      }

      var evaluation = (await Task.WhenAll(tasks)).ToList();
      var list = new List<bool>();
      
      foreach(var e in evaluation)
      {
        list.Add(await e.Value());
      }

      return list;
    }
    public static ExecutionContext ResolveMaps(this ExecutionContext context, object state, IList<Map> maps, IList<Variable> variables)
    {
      var vars = variables.ToList();
      if (maps == null)
      {
        return new ExecutionContext(state, context.GlobalServiceProvider, context.LocalServiceProvider, context.GlobalVariables, vars);
      }     
      
      foreach (var map in maps)
      {
        vars.Add(map.Resolve(context));
      }

      return new ExecutionContext(state, context.GlobalServiceProvider, context.LocalServiceProvider, context.GlobalVariables, vars);
    }

    public static ExecutionContext ResolveMaps(this ExecutionContext context, IList<Map> maps, IList<Variable> variables)
    {
      return context.ResolveMaps(context.CurrentState, maps, variables);
    }

    public  static object[] ResolveInputs(this IList<Input> inputs, ExecutionContext context)
    {
      var _inputs = new List<object>();

      foreach (var i in inputs)
      {
        _inputs.Add(i.Resolve(context));
      }

      return _inputs.ToArray();
    }
  }
}
