// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Common;
using TplWorkflow.Core.Inputs;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Methods
{
  public class InstanceMethod : Method
  {
    public object Instance { get; }
    Func<MethodInfo, object, IList<object>, Task> GetTask { get; }
    public InstanceMethod(MethodInfo info, object instance, IList<Input> inputs, Func<MethodInfo, object, IList<object>, Task> getTask) :base(info, inputs) 
    {
      Instance = instance;
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
      return GetTask(MethodInfo, Instance, inputs);
    }
  }
}
