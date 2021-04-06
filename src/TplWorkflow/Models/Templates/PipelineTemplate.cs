// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Models.Templates
{
  using TplWorkflow.Models.Templates.Interfaces;
  using System.Collections.Generic;

  public class PipelineTemplate : WorkflowDefinition, IKind, IMappable
  {
    public string Kind { get; set; }
    public int? Dop { get; set; }
    public ConditionTemplate Condition { get; set; }
    public IList<StepTemplate> Case { get; set; }
    public IList<StepTemplate> Steps { get; set; }
    public IDictionary<string, object> Variables { get; set; }
    public IList<MapTemplate> Maps { get; set; }
    public IDictionary<string, StepTemplate> When {get;set;}
    public InputTemplate Source { get; set; }
    public bool Disable { get; set; }
  }
}
