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

    public WorkflowLoader(IWorkflowStore workflowStore)
    {
      this.workflowStore = workflowStore;
    } 

    public WorkflowDefinition Register(WorkflowTemplat e template, TemplateContext context, IServiceCollection services)
    {
      template.Name.Required("Template should habe a name.");
      template.Version.Required(1, int.MaxValue, "Template should have a version.");
      context ??= new TemplateContext();

      services = services ?? new ServiceCollection();
      var sp = services
         .ConfigureDependency(template.Dependencies)
         .ConfigureVariableStore()
         .BuildServiceProvider();

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
