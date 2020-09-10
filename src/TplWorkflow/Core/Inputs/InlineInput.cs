using TplWorkflow.Core.Common;
using System;

namespace TplWorkflow.Core.Inputs
{
  public class InlineInput: Input
  {
    public InlineInput(Type type, object data)
      : base(type)
    {
      Data = data;
    }
    public object Data { get; }

    public override object Resolve(ExecutionContext context)
    {
      return Data;
    }
  }
}
