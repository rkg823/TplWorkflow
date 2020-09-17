// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using System;

namespace TplWorkflow.Exceptions
{
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
