// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Models.Templates
{
  using System.Collections.Generic;
  
  public class TemplateContext
  {
    public IList<PipelineTemplate> LinkedPipelines { get; }
    public IList<ConditionTemplate> LinkedConditions { get; }

    public TemplateContext(IList<PipelineTemplate> ptemplates)
    {
      LinkedConditions =  new List<ConditionTemplate>();
      LinkedPipelines = ptemplates ?? new List<PipelineTemplate>();
    }

    public TemplateContext(IList<PipelineTemplate> ptemplates, IList<ConditionTemplate> ctemplates)
    {
      LinkedConditions = ctemplates ?? new List<ConditionTemplate>();
      LinkedPipelines = ptemplates ?? new List<PipelineTemplate>();
    }

    public TemplateContext()
    {
      LinkedPipelines = new List<PipelineTemplate>();
      LinkedConditions = new List<ConditionTemplate>();
    }
  }
}
