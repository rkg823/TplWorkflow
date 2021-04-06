// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Outputs
{
  using TplWorkflow.Core.Common;
  using TplWorkflow.Exceptions;
  using TplWorkflow.Stores.Interfaces;
  using System;

  public class StepOutput : Output
  {
    public Func<ExecutionContext, IVariableStore> ResolveVariables { get; }

    public StepOutput(string name, Func<ExecutionContext, IVariableStore> resolveVariables) : base(name)
    {
      ResolveVariables = resolveVariables;
    }

    public override bool Resolve(ExecutionContext context)
    {
      var variables = ResolveVariables(context);

      if (!variables.Add(Name, context.CurrentState))
      {
        throw new WorkflowException($"Variable (name - {Name}) with same name already exist.");
      }

      return true;
    }
  }
}
