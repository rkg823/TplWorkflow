// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Extensions;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Outputs;
using System.Collections.Generic;
using System.Threading.Tasks;
using TplWorkflow.Core.Methods;

namespace TplWorkflow.Core.Steps
{
  public class AsyncStep : Step
  {
    public AsyncStep(string id, Method method, IList<Output> outputs, Condition condition) : base(id, condition)
    {
      Method = method;
      Outputs = outputs;
    }

    public Method Method { get; }
    public IList<Output> Outputs { get; }

    public override AsyncResult<object> Resolve(ExecutionContext context)
    {
      var tcs = new TaskCompletionSource<object>();
      var task = Method.Resolve(context);
      task.AttachCompletionHandler(tcs, (this, context));
      task.AttachErrorHandler(tcs);
      return new AsyncResult<object>(tcs.Task);
    }
  }
}
