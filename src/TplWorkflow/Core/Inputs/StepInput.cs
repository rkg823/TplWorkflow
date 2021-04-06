// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Inputs
{
  using TplWorkflow.Core.Common;
  using System;

  public class StepInput : Input
  {
    public StepInput(Type type) : base(type)
    {
    }

    public override object Resolve(ExecutionContext context)
    {
      return context.CurrentState;
    }
  }
}
