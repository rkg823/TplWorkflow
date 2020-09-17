// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Extensions;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Steps;
using System.Collections.Generic;
using System.Threading.Tasks;
using TplWorkflow.Core.Maps;

namespace TplWorkflow.Core.Pipelines
{
  public class ParallelPipeline: Pipeline
  {

    public ParallelPipeline(Condition condition, IList<Step> steps, IList<Variable> variables, IList<Map> maps)
     : base( condition, variables, maps)
    {
      Steps = steps;
    }
    public IList<Step> Steps { get; }
    public override async Task<AsyncResult<object>> Resolve(ExecutionContext context)
    {
      if (await this.ShouldSkip(context))
      {
        return new AsyncResult<object>(new List<object>());
      }
      var taskList = new List<Task<object>>();
      var _context = context.ResolveMaps(Maps, Variables);
      foreach (var step in Steps)
      {
        if (await step.ShouldSkip(_context))
        {
          continue;
        }
        var tResult = step.Resolve(_context);
        taskList.Add(tResult.Value());
      }
      var result = await Task.WhenAll(taskList);
      return new AsyncResult<object>(result);
    }
  }
}
