// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Exceptions
{
  using System;

  public class WorkflowException : Exception
  {
    public WorkflowException(string message) : base(message)
    {
    }

    public WorkflowException(string message, Exception inner) : base(message, inner)
    {
    }
  }
}
