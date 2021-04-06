// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Inputs
{
  using TplWorkflow.Core.Common;
  using System;

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
