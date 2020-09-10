using TplWorkflow.Core.Common;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Maps;
using TplWorkflow.Core.Steps;
using TplWorkflow.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Pipelines
{
  public class IfPipeline : Pipeline
  {
    public IfPipeline(
      Condition condition,
      IList<Variable> variables,
      IList<Map> maps,
      bool disable,
      Step trueStep,
      Step falseStep)
      : base(condition, variables, maps)
    {
      TrueStep = trueStep;
      FalseStep = falseStep;
      Dissable = disable;
    }

    public bool Dissable { get; }
    public Step TrueStep { get; }
    public Step FalseStep { get; }
    public async override Task<AsyncResult<object>> Resolve(ExecutionContext context)
    {
      var _context = context.ResolveMaps(Maps, Variables);
      var task =  await Condition.Resolve(_context);
      var value = await task.Value();
      if (value)
      {
        return TrueStep.Resolve(_context);
      }
      else
      {
        return FalseStep.Resolve(_context);
      }
    }
  }
}
