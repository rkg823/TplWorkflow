using TplWorkflow.Core.Common;
using System;

namespace TplWorkflow.Core.Inputs
{
  public abstract class Input
  {
    public Type DataType { get; }
    protected Input(Type type)
    {
      DataType = type;
    }
    public abstract object Resolve(ExecutionContext context); 
  }
}
