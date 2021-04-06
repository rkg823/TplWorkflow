// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Pipelines
{
  using TplWorkflow.Extensions;
  using TplWorkflow.Core.Conditions;
  using TplWorkflow.Core.Common;
  using TplWorkflow.Core.Steps;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using TplWorkflow.Core.Maps;
  using TplWorkflow.Models;

  public class SequentialPipeline : Pipeline
  {
    public IList<Step> Steps { get; }

    public SequentialPipeline(Condition condition, IList<Step> steps, IList<Variable> variables, IList<Map> maps) : base(condition, variables, maps)
    {
      Steps = steps;
    }

    public override async Task<AsyncResult<object>> Resolve(ExecutionContext context)
    {
      if (await this.ShouldSkip(context))
      {
        return new AsyncResult<object>(null);
      }

      var _context = context.ResolveMaps(Maps, Variables);

      foreach (var step in Steps)
      {
        if (await step.ShouldSkip(_context))
        {
          continue;
        }

        var task = step.Resolve(_context);
        var state = await task.Value();

        if (state is VoidReturn)
        {
          continue;
        }

        _context = new ExecutionContext(state, _context.ServiceProvider, _context.GlobalVariables, _context.PipelineVariables);
      }

      return new AsyncResult<object>(_context.CurrentState);
    }

  }
}
