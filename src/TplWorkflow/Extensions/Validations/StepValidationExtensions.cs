// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Exceptions;
using TplWorkflow.Models.Templates;
using System.Collections.Generic;
using System.Linq;

namespace TplWorkflow.Extensions.Validations
{
  public static class StepValidationExtensions
  {
    public static void RequiredMethod(this StepTemplate template)
    {
      template.Method.Required("Task step should have a method defined");
      var list = new List<string> { template.Instance, template.Contract, template.Static };
      if(list.Count(e => e != null) == 0)
        throw new WorkflowException("Step must have one method type of either Instance, Static, or Contract.");
      if (list.Count(e => e != null) > 1)
        throw new WorkflowException("Step cannot have more than one method type of Instance, Static, or Contract.");
    }
  }
}
