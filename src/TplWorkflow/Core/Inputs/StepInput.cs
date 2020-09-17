// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Common;
using System;

namespace TplWorkflow.Core.Inputs
{
  public class StepInput: Input
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
