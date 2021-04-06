// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Inputs
{
  using TplWorkflow.Core.Common;
  using TplWorkflow.Stores.Interfaces;
  using System;

  public class VariableInput : Input
  {
    public string Name { get; set; }
    public Func<ExecutionContext, IVariableStore> ResolveVariables { get; }

    public VariableInput(string name, Type type, Func<ExecutionContext, IVariableStore> resolveVariables) : base(type)
    {
      Name = name;
      ResolveVariables = resolveVariables;
    }

    public override object Resolve(ExecutionContext context)
    {
      var data = ResolveVariables(context).Get(Name);
      return data;
    }
  }
}
