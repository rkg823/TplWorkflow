// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Common;
using TplWorkflow.Core.Steps;
using System;
using System.Threading.Tasks;

namespace TplWorkflow.Extensions
{
  public static class AsyncStepExtensions
  {
    public static void AttachCompletionHandler(this Task task, TaskCompletionSource<object> tcs, (AsyncStep step, ExecutionContext context) beg)
    {
      task.ContinueWith((t, data) =>
      {
        try
        {
          var _beg = ((AsyncStep step, ExecutionContext context))data;
          var _task = t as dynamic;
          if (_beg.step.Outputs != null)
          {
            var _context = new ExecutionContext(_task.Result, _beg.context.ServiceProvider, _beg.context.GlobalVariables, _beg.context.PipelineVariables);
            foreach (var o in _beg.step.Outputs)
            {
              o.Resolve(_context);
            }
          }
          tcs.SetResult(_task.Result);
        }
        catch (Exception ex)
        {
          tcs.SetException(ex);
        }
      }, beg, TaskContinuationOptions.OnlyOnRanToCompletion);
    }
    public static void AttachErrorHandler(this Task task, TaskCompletionSource<object> tcs)
    {
      task.ContinueWith((t) =>
      {
        tcs.SetException(t.Exception);
      },
     TaskContinuationOptions.OnlyOnFaulted);
    }
  }
}
