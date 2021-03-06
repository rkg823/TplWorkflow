﻿// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow
{
  using System;
  using System.Threading.Tasks;
  using TplWorkflow.Stores.Interfaces;
  using TplWorkflow.Extensions.Validations;
  using TplWorkflow.Exceptions;
  using TplWorkflow.Core.Common;
  using TplWorkflow.Services.Interfaces;
  using System.Collections.Generic;
  using System.Linq;
  using TplWorkflow.Models;

  public class WorkflowHost : IWorkflowHost
  {
    private readonly IWorkflowStore workflowStore;

    private readonly IServiceProvider globalServicrProvider;

    public WorkflowHost(IWorkflowStore workflowStore, IServiceProvider globalServicrProvider)
    {
      this.workflowStore = workflowStore;
      this.globalServicrProvider = globalServicrProvider;
    }

    #region Start
    public async Task<object> StartAsync(string name, int version)
    {
      var (wf, sp) = GetWorkflow(name, version);
      var result = await InvokeAsync(wf, sp);

      return await result.Value();
    }

    public async Task<object> StartAsync(string name, int version, object data)
    {
      data.Required("Data is required to invoke workflow");

      var (wf, sp) = GetWorkflow(name, version);
      var result = await InvokeAsync(wf, sp, data);

      return await result.Value();
    }

    #endregion

    public WorkflowDefinition Get(string name, int version)
    {
      var (workflow, _) = workflowStore.Get(name, version);

      if (workflow == null)
      {
        return null;
      }

      return new WorkflowDefinition { Name = workflow.Name, Version = workflow.Version };
    }
    public IList<WorkflowDefinition> Get()
    {
      return workflowStore.Get().Select(e =>
      new WorkflowDefinition
      {
        Name = e.workflow.Name,
        Version = e.workflow.Version
      }).ToList();
    }

    public bool Contains(string name, int version)
    {
      return workflowStore.Contains(name, version);
    }

    public bool Clear()
    {
      return workflowStore.Clear();
    }

    public bool Remove(string name, int version)
    {
      return workflowStore.Remove(name, version);
    }

    #region Private
    private (Core.WorkflowInstance wf, IServiceProvider sp) GetWorkflow(string name, int version)
    {
      name.Required("Name is required to invoke workflow");
      var data = workflowStore.Get(name, version);

      if (data.workflow == null || data.provider == null)
      {
        throw new WorkflowrRgistrationException(name, version);
      }

      return data;
    }
    private Task<AsyncResult<object>> InvokeAsync(Core.WorkflowInstance workFlow, IServiceProvider localServiceProvider, object data = null)
    {
      var context = new ExecutionContext(data, globalServicrProvider, localServiceProvider, workFlow.Variables);

      return workFlow.Pipeline.Resolve(context);
    }
    #endregion

  }
}
