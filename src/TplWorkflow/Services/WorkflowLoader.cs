using TplWorkflow.Extensions;
using TplWorkflow.Extensions.Mappers;
using TplWorkflow.Extensions.Validations;
using TplWorkflow.Models;
using TplWorkflow.Models.Templates;
using TplWorkflow.Services.Interfaces;
using TplWorkflow.Stores.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TplWorkflow.Services
{
  public partial class WorkflowLoader: IWorklowLoader
  {
    private readonly IWorkflowStore workflowStore;
    public WorkflowLoader(IWorkflowStore workflowStore)
    {
      this.workflowStore = workflowStore;
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
      services = services ?? new ServiceCollection();
      context = context ?? new TemplateContext();
      var sp = services
        .ConfigureDependency(template.Dependencies)
        .ConfigureStore(workflowStore)
        .BuildServiceProvider(); 
      var wf = template.Map(context);
      workflowStore.Update((wf, sp));
      return template as WorkflowDefinition;
    }
  }
}
