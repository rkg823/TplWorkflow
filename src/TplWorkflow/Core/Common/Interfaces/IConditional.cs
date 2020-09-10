using TplWorkflow.Core.Conditions;

namespace TplWorkflow.Core.Common.Interfaces
{
  public interface IConditional
  {
    Condition Condition { get; }
  }
}
