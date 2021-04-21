// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Services
{
  using TplWorkflow.Extensions;
  using TplWorkflow.Extensions.Mappers;
  using TplWorkflow.Extensions.Validations;
  using TplWorkflow.Models;
  using TplWorkflow.Models.Templates;
  using TplWorkflow.Stores.Interfaces;
  using Microsoft.Extensions.DependencyInjection;
  using System;

  public partial class WorkflowLoader: IWorkflowLoader
  {
    private readonly IWorkflowStore workflowStore;

    private readonly IServiceProvider serviceProvider;

    public WorkflowLoader(IWorkflowStore workflowStore, IServiceProvider serviceProvider)
    {
      this.workflowStore = workflowStore;
      this.serviceProvider = serviceProvider;
    }

    public WorkflowDefinition From(Func<WorkflowTemplate> factory, Action<ServiceCollection> configureServices)
    {
      var sc = new ServiceCollection();
      configureServices(sc);
      var context = new TemplateContext();

      return Register(factory(),context, sc);
    }

    public WorkflowDefinition From(WorkflowTemplate template)
    {
      return Register(template);
    }

    public WorkflowDefinition From(WorkflowTemplate template, Action<ServiceCollection> configureServices)
    {
      var sc = new ServiceCollection();
      configureServices(sc);
      var context = new TemplateContext();

      return Register(template, context, sc);
    }

    private WorkflowDefinition Register(WorkflowTemplate template, TemplateContext context = null, IServiceCollection services = null)
    {
      template.Name.Required("Template should habe a name.");
      template.Version.Required(1, int.MaxValue, "Template should have a version.");
      context = context ?? new TemplateContext();

      IServiceProvider sp;
      if (services == null)
      {
        sp = serviceProvider;
      }
      else
      {
        sp = services
         .ConfigureDependency(template.Dependencies)
         .ConfigureStore(workflowStore)
         .BuildServiceProvider();     
      }

      var wf = template.Map(context);
      workflowStore.Update((wf, sp));

      return template;
    }
  }
}
