using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Core.Maps;
using TplWorkflow.Core.Steps;
using TplWorkflow.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplWorkflow.Models;

namespace TplWorkflow.Core.Pipelines
{
  public class ParallelForeachPipeline: Pipeline
  {
    public ParallelForeachPipeline(
      Condition condition, 
      IList<Step> steps, 
      IList<Variable> variables, 
      IList<Map> maps, 
      Input source, 
      int dop)
     : base(condition, variables, maps)
    {
      Source = source;
      Dop = dop;
      Steps = steps;
    }
    public IList<Step> Steps { get; }
    public Input Source { get; }
    public int Dop { get; }

    public override async Task<AsyncResult<object>> Resolve(ExecutionContext context)
    {
      if (await this.ShouldSkip(context))
      {
        return new AsyncResult<object>(new List<object>());
      }
      var source = Source.Resolve(context) as IEnumerable<object>;
      async Task<object[]> AwaitPartition<T>(IEnumerator<T> partition)
      {
        var states = new List<object>();
        using (partition)
        {
          while (partition.MoveNext())
          {
              var _context = context.ResolveMaps(partition.Current, Maps, Variables);
              foreach (var step in Steps)
              {
                if(await step.ShouldSkip(_context))
                  {
                    continue;
                  }
                var result = step.Resolve(_context);
                var state = await result.Value();
                if (state is VoidReturn)
                {
                  continue;
                }
              _context = new ExecutionContext(state, _context.ServiceProvider, _context.GlobalVariables, _context.PipelineVariables);
              }
              states.Add(_context.CurrentState);
          }
          return states.ToArray();
        }
      }

      var partitionOutputs = await Task.WhenAll(
          Partitioner
              .Create(source)
              .GetPartitions(Dop)
              .AsParallel()
              .Select(p => AwaitPartition(p)));
      var output = partitionOutputs.SelectMany(e => e).ToList();
      return new AsyncResult<object>(output);
    }

   

  }
}
