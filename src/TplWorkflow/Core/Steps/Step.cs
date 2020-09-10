using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Common.Interfaces;

namespace TplWorkflow.Core.Steps
{
  public abstract class Step: IConditional
  {
    public string Id { get;}
    public Condition Condition { get; }
    protected Step(string id, Condition condition)
    {
      Id = id;
      Condition = condition;
    }

    public abstract AsyncResult<object> Resolve(ExecutionContext context);
  }
}
