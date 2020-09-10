using TplWorkflow.Core.Common;

namespace TplWorkflow.Core.Outputs
{
  public abstract class Output
  {
    public string Name { get; }
    protected Output(string name)
    {
      Name = name;
    }
    public abstract bool Resolve(ExecutionContext context);
  }
}
