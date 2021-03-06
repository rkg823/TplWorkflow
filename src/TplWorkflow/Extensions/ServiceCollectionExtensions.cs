﻿// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Extensions
{
  using TplWorkflow.Exceptions;
  using TplWorkflow.Models.Templates;
  using TplWorkflow.Services;
  using TplWorkflow.Services.Interfaces;
  using TplWorkflow.Stores;
  using TplWorkflow.Stores.Interfaces;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.DependencyInjection.Extensions;
  using System;
  using System.Collections.Generic;

  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddWorkflow(this IServiceCollection services)
    {
      services.AddSingleton<IWorkflowHost, WorkflowHost>();
      services.AddSingleton<IWorkflowLoader, WorkflowLoader>();
      services.AddSingleton<IWorkflowStore, WorkflowMemoryStore>();
      return services;
    }

    public static IServiceCollection ConfigureDependency(this IServiceCollection serviceCollection, IList<DependencyTemplate> dependencies)
    {
      if (dependencies == null || dependencies.Count == 0)
      {
        return serviceCollection;
      }
       
      var sdList = new List<ServiceDescriptor>();

      foreach (var dep in dependencies)
      {
        var contract = Type.GetType(dep.Contract);
        var implemetation = Type.GetType(dep.Implementation);
        
        if (!Enum.TryParse(dep.Lifetime, out ServiceLifetime lifetime))
        {
          throw new WorkflowException("Invalid lifetime for dependency");
        }

        var sd = new ServiceDescriptor(contract, implemetation, lifetime);
        sdList.Add(sd);
      }
     
      serviceCollection.Add(sdList);

      return serviceCollection;
    }

    public static IServiceCollection ConfigureVariableStore(this IServiceCollection serviceCollection)
    {
      serviceCollection.AddTransient<IVariableStore, VariableMemoryStore>();

      return serviceCollection;
    }
  }
}
