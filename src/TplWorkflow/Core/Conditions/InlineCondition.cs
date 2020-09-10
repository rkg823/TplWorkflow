using TplWorkflow.Core.Common;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Conditions
{
  public class InlineCondition: Condition
  {
    public InlineCondition(bool data)
    {
      Data = data;
    }
    public bool Data { get; }

    public override Task<AsyncResult<bool>> Resolve(ExecutionContext context)
    {
      return Task.FromResult(new AsyncResult<bool>(Data));
    }
  }
}
