using TplWorkflow.Core.Common;
using TplWorkflow.Core.Common.Interfaces;
using TplWorkflow.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Conditions
{
  public class AndCondition : Condition, IMultiCondition
  {
    public IList<Condition> Conditions { get; }
    public AndCondition(IList<Condition> conditions)
    {
      Conditions = conditions;
    }
    public override async Task<AsyncResult<bool>> Resolve(ExecutionContext context)
    {
      var evaluated = !(await this.Evaluate(context)).Contains(false);
      return new AsyncResult<bool>(evaluated);
    }
  }
}
