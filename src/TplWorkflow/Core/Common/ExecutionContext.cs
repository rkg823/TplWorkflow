// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Common
{
  using TplWorkflow.Exceptions;
  using TplWorkflow.Stores.Interfaces;
  using Microsoft.Extensions.DependencyInjection;
  using System;
  using System.Collections.Generic;

  public class ExecutionContext
  {
    public IServiceProvider ServiceProvider { get; }
    public object CurrentState { get; }
    public IVariableStore GlobalVariables { get; }
    public IVariableStore PipelineVariables { get; }

    public ExecutionContext(object input, IServiceProvider serviceProvider)
    {
      ServiceProvider = serviceProvider;
      CurrentState = input;
      GlobalVariables = serviceProvider.GetService<IVariableStore>();
      PipelineVariables = serviceProvider.GetService<IVariableStore>();
    }

    public ExecutionContext(object input, IServiceProvider serviceProvider, IList<Variable> globalVariables)
    {
      ServiceProvider = serviceProvider;
      CurrentState = input;
      GlobalVariables = serviceProvider.GetService<IVariableStore>();
      PipelineVariables = serviceProvider.GetService<IVariableStore>();

      InitVariables(GlobalVariables, globalVariables);
    }

    public ExecutionContext(object input, IServiceProvider serviceProvider, IList<Variable> globalVariables, IList<Variable> pipelineVariables)
    {
      ServiceProvider = serviceProvider;
      CurrentState = input;
      GlobalVariables = serviceProvider.GetService<IVariableStore>();
      PipelineVariables = serviceProvider.GetService<IVariableStore>();

      InitVariables(GlobalVariables, globalVariables);
      InitVariables(PipelineVariables, pipelineVariables);
    }

    public ExecutionContext(object input, IServiceProvider serviceProvider, IVariableStore globalVariables)
    {
      ServiceProvider = serviceProvider;
      CurrentState = input;
      GlobalVariables = globalVariables ?? serviceProvider.GetService<IVariableStore>();
      PipelineVariables = serviceProvider.GetService<IVariableStore>();
    }

    public ExecutionContext(object input, IServiceProvider serviceProvider, IVariableStore globalVariables, IList<Variable> pipelineVariables)
    {
      ServiceProvider = serviceProvider;
      CurrentState = input;
      GlobalVariables = globalVariables ?? serviceProvider.GetService<IVariableStore>();
      PipelineVariables = serviceProvider.GetService<IVariableStore>();

      InitVariables(PipelineVariables, pipelineVariables);
    }

    public ExecutionContext(object input, IServiceProvider serviceProvider, IVariableStore globalVariables, IVariableStore pipelineVariables)
    {
      ServiceProvider = serviceProvider;
      CurrentState = input;
      GlobalVariables = globalVariables ?? serviceProvider.GetService<IVariableStore>();
      PipelineVariables = pipelineVariables ?? serviceProvider.GetService<IVariableStore>();
    }

    public ExecutionContext(IServiceProvider serviceProvider)
    {
      ServiceProvider = serviceProvider;
      GlobalVariables = serviceProvider.GetService<IVariableStore>();
    }

    private void InitVariables(IVariableStore store, IList<Variable> variables)
    {
      foreach (var variable in variables)
      {
        if (!store.Add(variable))
        {
          throw new WorkflowException($"Variable (name - {variable.Name}) can not be modified ater initilization.");
        }
      }
    }

  }
}
