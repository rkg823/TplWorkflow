using System.Collections.Generic;

namespace TplWorkflow.Models.Templates
{
  public class TemplateContext
  {
    public IList<PipelineTemplate> LinkedPipelines { get; }
    public IList<ConditionTemplate> LinkedConditions { get; }
    public TemplateContext(IList<PipelineTemplate> ptemplates)
    {
      this.LinkedConditions =  new List<ConditionTemplate>();
      this.LinkedPipelines = ptemplates ?? new List<PipelineTemplate>();
    }
    public TemplateContext(IList<PipelineTemplate> ptemplates, IList<ConditionTemplate> ctemplates)
    {
      this.LinkedConditions = ctemplates ?? new List<ConditionTemplate>();
      this.LinkedPipelines = ptemplates ?? new List<PipelineTemplate>();
    }
    public TemplateContext()
    {
      this.LinkedPipelines = new List<PipelineTemplate>();
      this.LinkedConditions = new List<ConditionTemplate>();
    }
  }
}
