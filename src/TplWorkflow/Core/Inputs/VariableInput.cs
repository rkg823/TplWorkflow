using TplWorkflow.Core.Common;
using TplWorkflow.Stores.Interfaces;
using System;

namespace TplWorkflow.Core.Inputs
{
  public class VariableInput : Input
  {
    public VariableInput(string name, Type type, Func<ExecutionContext, IVariableStore> resolveVariables) : base(type)
    {
      Name = name;
      ResolveVariables = resolveVariables;
    }
    public string Name { get; set; }

    public Func<ExecutionContext, IVariableStore> ResolveVariables { get; }

    public override object Resolve(ExecutionContext context)
    {
      var data = ResolveVariables(context).Get(Name);
      return data;
    }
  }
}
