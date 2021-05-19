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
  using TplWorkflow.Core.Inputs;
  using TplWorkflow.Models;

  public class ForEachPipeline : Pipeline
  {
    public IList<Step> Steps { get; }
    public Input Source { get; }

    public ForEachPipeline(Condition condition, IList<Step> steps, IList<Variable> variables, IList<Map> maps, Input source) : base(condition, variables, maps)
    {
      Source = source;
      Steps = steps;
    }

    public override async Task<AsyncResult<object>> Resolve(ExecutionContext context)
    {
      if (await this.ShouldSkip(context))
      {
        return new AsyncResult<object>(new List<object>());
      }

      var source = Source.Resolve(context) as IEnumerable<object>;
      var states = new List<object>();

      foreach (var item in source)
      {
        var _context = context.ResolveMaps(item, Maps, Variables);

        foreach (var step in Steps)
        {
          if (await step.ShouldSkip(_context))
          {
            continue;
          }

          var result = step.Resolve(_context);
          var state = await result.Value();

          if (state is VoidReturn)
          {
            continue;
          }

          _context = new ExecutionContext(state, _context.GlobalServiceProvider, _context.LocalServiceProvider, _context.GlobalVariables, _context.PipelineVariables);
        }

        states.Add(_context.CurrentState);
      }

      return new AsyncResult<object>(states);
    }
  }
}
