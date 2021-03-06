﻿// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Steps
{
  using TplWorkflow.Exceptions;
  using TplWorkflow.Core.Conditions;
  using TplWorkflow.Core.Common;
  using TplWorkflow.Stores.Interfaces;
  using Microsoft.Extensions.DependencyInjection;
  using System.Threading.Tasks;

  public class WorkflowStep : Step
  {
    public string Name { get; }
    public int Version { get; }

    public WorkflowStep(string id, string name, int version, Condition condition) : base(id, condition)
    {
      Name = name;
      Version = version;
    }

    public override AsyncResult<object> Resolve(ExecutionContext context)
    {
      var tcs = new TaskCompletionSource<object>();
      var store = context.GlobalServiceProvider.GetRequiredService(typeof(IWorkflowStore)) as IWorkflowStore;
      var (workflow, provider) = store.Get(Name, Version);

      if (workflow == null || provider == null)
      {
        throw new WorkflowrRgistrationException(Name, Version);
      }

      var _context = new ExecutionContext(context.CurrentState, context.GlobalServiceProvider, provider, workflow.Variables);
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
