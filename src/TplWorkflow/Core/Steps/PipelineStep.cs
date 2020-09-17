// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Pipelines;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Steps
{
  public class PipelineStep: Step
  {
    public PipelineStep(string id, Pipeline pipeline, Condition condition): base(id, condition)
    {
      Pipeline = pipeline;
    }
    public Pipeline Pipeline { get; }
    public override AsyncResult<object> Resolve(ExecutionContext context)
    {
      var tcs = new TaskCompletionSource<object>();
      var task = Pipeline.Resolve(context);
      task.ContinueWith((t, beg) =>
      {
        var value = t.Result.Value();
        if(value == null)
        {
          tcs.SetResult(null);
          return;
        }
        value.ContinueWith(ct =>
        {
          tcs.SetResult(ct.Result);
        },TaskContinuationOptions.OnlyOnRanToCompletion);
        value.ContinueWith(ct =>
        {
          tcs.SetException(ct.Exception);
        }, TaskContinuationOptions.OnlyOnFaulted);

      }, TaskContinuationOptions.OnlyOnRanToCompletion);
      task.ContinueWith((t) =>
      {
        tcs.SetException(t.Exception);
      }, TaskContinuationOptions.OnlyOnFaulted);
      return new AsyncResult<object>(tcs.Task);
    }
  }
}
