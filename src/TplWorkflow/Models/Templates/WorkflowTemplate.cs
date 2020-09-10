using System.Collections.Generic;

namespace TplWorkflow.Models.Templates
{
  public class WorkflowTemplate: WorkflowDefinition
  {
    public string Description { get; set; }
    public IList<DependencyTemplate> Dependencies { get; set; }
    public IDictionary<string, object> Variables { get; set; }
    public IList<PipelineTemplate> Pipelines { get; set; }
  }
}
