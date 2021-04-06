// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Models.Templates
{
  using System.Collections.Generic;

  public class WorkflowTemplate: WorkflowDefinition
  {
    public string Description { get; set; }
    public IList<DependencyTemplate> Dependencies { get; set; }
    public IDictionary<string, object> Variables { get; set; }
    public IList<PipelineTemplate> Pipelines { get; set; }
  }
}
