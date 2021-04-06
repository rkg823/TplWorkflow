// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Inputs
{
  using TplWorkflow.Core.Common;
  using System;

  public class InlineInput : Input
  {
    public object Data { get; }

    public InlineInput(Type type, object data) : base(type)
    {
      Data = data;
    }

    public override object Resolve(ExecutionContext context)
    {
      return Data;
    }
  }
}
