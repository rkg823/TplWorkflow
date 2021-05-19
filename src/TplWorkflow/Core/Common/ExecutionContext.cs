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
    public IServiceProvider LocalServiceProvider { get; }

    public IServiceProvider GlobalServiceProvider { get; }

    public object CurrentState { get; }

    public IVariableStore GlobalVariables { get; }

    public IVariableStore PipelineVariables { get; }

    public ExecutionContext(object input, IServiceProvider globalServiceProvider, IServiceProvider localServiceProvider)
    {
      GlobalServiceProvider = globalServiceProvider;
      LocalServiceProvider = localServiceProvider;
      CurrentState = input;
      GlobalVariables = localServiceProvider.GetRequiredService<IVariableStore>();
      PipelineVariables = localServiceProvider.GetRequiredService<IVariableStore>();
    }

    public ExecutionContext(object input, IServiceProvider globalServiceProvider, IServiceProvider localServiceProvider, IList<Variable> globalVariables)
    {
      GlobalServiceProvider = globalServiceProvider;
      LocalServiceProvider = localServiceProvider;
      CurrentState = input;
      GlobalVariables = localServiceProvider.GetRequiredService<IVariableStore>();
      PipelineVariables = localServiceProvider.GetRequiredService<IVariableStore>();

      InitVariables(GlobalVariables, globalVariables);
    }

    public ExecutionContext(object input, IServiceProvider globalServiceProvider, IServiceProvider localServiceProvider, IList<Variable> globalVariables, IList<Variable> pipelineVariables)
    {
      GlobalServiceProvider = globalServiceProvider;
      LocalServiceProvider = localServiceProvider;
      CurrentState = input;
      GlobalVariables = localServiceProvider.GetRequiredService<IVariableStore>();
      PipelineVariables = localServiceProvider.GetRequiredService<IVariableStore>();

      InitVariables(GlobalVariables, globalVariables);
      InitVariables(PipelineVariables, pipelineVariables);
    }

    public ExecutionContext(object input, IServiceProvider globalServiceProvider, IServiceProvider localServiceProvider, IVariableStore globalVariables)
    {
      GlobalServiceProvider = globalServiceProvider;
      LocalServiceProvider = localServiceProvider;
      CurrentState = input;
      GlobalVariables = globalVariables ?? localServiceProvider.GetRequiredService<IVariableStore>();
      PipelineVariables = localServiceProvider.GetRequiredService<IVariableStore>();
    }

    public ExecutionContext(object input, IServiceProvider globalServiceProvider, IServiceProvider localServiceProvider, IVariableStore globalVariables, IList<Variable> pipelineVariables)
    {
      GlobalServiceProvider = globalServiceProvider;
      LocalServiceProvider = localServiceProvider;
      CurrentState = input;
      GlobalVariables = globalVariables ?? localServiceProvider.GetRequiredService<IVariableStore>();
      PipelineVariables = localServiceProvider.GetRequiredService<IVariableStore>();

      InitVariables(PipelineVariables, pipelineVariables);
    }

    public ExecutionContext(object input, IServiceProvider globalServiceProvider, IServiceProvider localServiceProvider, IVariableStore globalVariables, IVariableStore pipelineVariables)
    {
      GlobalServiceProvider = globalServiceProvider;
      LocalServiceProvider = localServiceProvider;
      CurrentState = input;
      GlobalVariables = globalVariables ?? localServiceProvider.GetRequiredService<IVariableStore>();
      PipelineVariables = pipelineVariables ?? localServiceProvider.GetRequiredService<IVariableStore>();
    }

    public ExecutionContext(IServiceProvider globalServiceProvider, IServiceProvider localServiceProvider)
    {
      GlobalServiceProvider = globalServiceProvider;
      LocalServiceProvider = localServiceProvider;
      GlobalVariables = localServiceProvider.GetRequiredService<IVariableStore>();
    }

    public object GetRequiredService(Type type)
    {
      if (LocalServiceProvider != null)
      {
        var service = LocalServiceProvider.GetService(type);

        if(service != null)
        {
          return service;
        }
      }

      if(GlobalServiceProvider != null)
      {
        var service = GlobalServiceProvider.GetService(type);

        if (service != null)
        {
          return service;
        }
      }

      throw new WorkflowException($"${type.AssemblyQualifiedName} is not registerd.");  
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
