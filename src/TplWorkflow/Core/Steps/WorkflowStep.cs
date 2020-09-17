// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Exceptions;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Common;
using TplWorkflow.Stores.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Steps
{
  public class WorkflowStep : Step
  {
    public WorkflowStep(string id, string name, int version, Condition condition) : base(id, condition)
    {
      Name = name;
      Version = version;
    }
    public string Name { get; }
    public int Version { get; }
    public override AsyncResult<object> Resolve(ExecutionContext context)
    {
      var tcs = new TaskCompletionSource<object>();
      var store = context.ServiceProvider.GetService<IWorkflowStore>();
      var (workflow, provider) = store.Get(Name, Version);
      if (workflow == null || provider == null)
      {
          throw new WorkflowrRgistrationException(Name, Version);
      }
      var _context = new ExecutionContext(context.CurrentState, provider, workflow.Variables);
      var task = workflow.Pipeline.Resolve(_context);
      task.ContinueWith((t, beg) =>
      {
        t.Result.Value().ContinueWith(ct =>
        {
          tcs.SetResult(ct.Result);
        }, TaskContinuationOptions.OnlyOnRanToCompletion);
        t.Result.Value().ContinueWith(ct =>
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
