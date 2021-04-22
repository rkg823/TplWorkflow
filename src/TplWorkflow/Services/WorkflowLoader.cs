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
  using TplWorkflow.Services.Interfaces;

  public partial class WorkflowLoader : IWorkflowLoader
  {
    private readonly IWorkflowStore workflowStore;

    private readonly IServiceProvider serviceProvider;

    public WorkflowLoader(IWorkflowStore workflowStore, IServiceProvider serviceProvider)
    {
      this.workflowStore = workflowStore;
      this.serviceProvider = serviceProvider;
    } 

    public WorkflowDefinition Register(WorkflowTemplate template, TemplateContext context, IServiceCollection services)
    {
      template.Name.Required("Template should habe a name.");
      template.Version.Required(1, int.MaxValue, "Template should have a version.");
      context ??= new TemplateContext();

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

    public WorkflowDefinition Register(WorkflowTemplate template, TemplateContext context)
    {
      return Register(template, context, null);
    }

    public WorkflowDefinition Register(WorkflowTemplate template)
    {
      return Register(template, null, null);
    }
  }
}
