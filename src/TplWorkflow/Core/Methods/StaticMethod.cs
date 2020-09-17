// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Common;
using TplWorkflow.Core.Inputs;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Methods
{
  public class StaticMethod : Method
  {
    Func<MethodInfo, IList<object>, Task> GetTask { get; }
    public StaticMethod(MethodInfo info, IList<Input> inputs, Func<MethodInfo, IList<object>, Task> getTask) : base(info, inputs)
    {
      GetTask = getTask;
    }

    public override Task Resolve(ExecutionContext context)
    {
      var inputs = new List<object>();
      if (Inputs != null)
      {
        foreach (var input in Inputs)
        {
          inputs.Add(input.Resolve(context));
        }
      }
      return GetTask(MethodInfo, inputs);
    }
  }
}

