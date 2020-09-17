// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Exceptions;
using TplWorkflow.Stores.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TplWorkflow.Core.Common
{
  public class ExecutionContext
  {
    public IServiceProvider ServiceProvider { get; }
    public object CurrentState { get; }
    public IVariableStore GlobalVariables { get; }
    public IVariableStore PipelineVariables { get; }

    public ExecutionContext(object input, IServiceProvider serviceProvider)
    {
      this.ServiceProvider = serviceProvider;
      this.CurrentState = input;
      this.GlobalVariables = serviceProvider.GetService<IVariableStore>();
      this.PipelineVariables = serviceProvider.GetService<IVariableStore>();
    }
    public ExecutionContext(object input, IServiceProvider serviceProvider, IList<Variable> globalVariables)
    {
      this.ServiceProvider = serviceProvider;
      this.CurrentState = input;
      this.GlobalVariables =  serviceProvider.GetService<IVariableStore>();
      this.PipelineVariables = serviceProvider.GetService<IVariableStore>();
      InitVariables(this.GlobalVariables, globalVariables);
    }

    public ExecutionContext(object input, IServiceProvider serviceProvider, IList<Variable> globalVariables, IList<Variable> pipelineVariables)
    {
      this.ServiceProvider = serviceProvider;
      this.CurrentState = input;
      this.GlobalVariables = serviceProvider.GetService<IVariableStore>();
      this.PipelineVariables = serviceProvider.GetService<IVariableStore>();
      InitVariables(this.GlobalVariables, globalVariables);
      InitVariables(this.PipelineVariables, pipelineVariables);
    }

    public ExecutionContext(object input, IServiceProvider serviceProvider, IVariableStore globalVariables)
    {
      this.ServiceProvider = serviceProvider;
      this.CurrentState = input;
      this.GlobalVariables = globalVariables ?? serviceProvider.GetService<IVariableStore>();
      this.PipelineVariables = serviceProvider.GetService<IVariableStore>();
    }

    public ExecutionContext(object input, IServiceProvider serviceProvider, IVariableStore globalVariables, IList<Variable> pipelineVariables)
    {
      this.ServiceProvider = serviceProvider;
      this.CurrentState = input;
      this.GlobalVariables = globalVariables?? serviceProvider.GetService<IVariableStore>();
      this.PipelineVariables = serviceProvider.GetService<IVariableStore>();
      InitVariables(this.PipelineVariables, pipelineVariables);
    }

    public ExecutionContext(object input, IServiceProvider serviceProvider, IVariableStore globalVariables, IVariableStore pipelineVariables)
    {
      this.ServiceProvider = serviceProvider;
      this.CurrentState = input;
      this.GlobalVariables = globalVariables ?? serviceProvider.GetService<IVariableStore>();
      this.PipelineVariables = pipelineVariables ?? serviceProvider.GetService<IVariableStore>();
    }

    public ExecutionContext(IServiceProvider serviceProvider)
    {
      this.ServiceProvider = serviceProvider;
      this.GlobalVariables = serviceProvider.GetService<IVariableStore>();
    }

    private void InitVariables(IVariableStore store, IList<Variable> variables)
    {
      foreach(var variable in variables)
      {
        if (!store.Add(variable)) 
        {
          throw new WorkflowException($"Variable (name - {variable.Name}) can not be modified ater initilization.");
        }
      }
    }

  }
}
