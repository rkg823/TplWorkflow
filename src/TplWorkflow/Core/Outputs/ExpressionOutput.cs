﻿// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Outputs
{
  using TplWorkflow.Core.Common;
  using TplWorkflow.Core.Inputs;
  using TplWorkflow.Exceptions;
  using TplWorkflow.Stores.Interfaces;
  using System;
  using System.Collections.Generic;

  public class ExpressionOutput : Output
  {
    public Delegate Method { get; }
    public IList<Input> Inputs { get; }
    public Func<ExecutionContext, IVariableStore> ResolveVariables { get; }

    public ExpressionOutput(string name, Delegate method, IList<Input> inputs, Func<ExecutionContext, IVariableStore> resolveVariables) : base(name)
    {
      Method = method;
      Inputs = inputs;
      ResolveVariables = resolveVariables;
    }

    public override bool Resolve(ExecutionContext context)
    {
      var inputs = new List<object>();

      foreach (var input in Inputs)
      {
        inputs.Add(input.Resolve(context));
      }

      var result = Method.DynamicInvoke(inputs.ToArray());
      var variables = ResolveVariables(context);

      if (!variables.Add(Name, result))
      {
        throw new WorkflowException($"Variable (name - {Name}) with same name already exist.");
      }

      return true;
    }
  }
}
