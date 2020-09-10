using TplWorkflow.Extensions.Validations;
using TplWorkflow.Models.Templates;
using System.Linq;

namespace TplWorkflow.Extensions.Mappers
{
  public static class WorkflowMapperExtension
  {
    public static Core.WorkflowInstance Map(this WorkflowTemplate template, TemplateContext context)
    {
      template.Name.Required("Workflow should have a name.");
      template.Version.Required(0, int.MaxValue, "Workflow should have a version.");
      template.Pipelines.Required("Workflow should have a pipeline.");
      var startPipeline = template.Pipelines.First();
      var pipeline = startPipeline.Map(context);
      var variabes = template.Variables.Map();
      return new Core.WorkflowInstance(template.Name, template.Version, template.Description, pipeline, variabes);
    }  
  }
}
