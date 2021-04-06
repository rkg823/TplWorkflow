// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Models.Templates
{
  using TplWorkflow.Models.Templates.Interfaces;
  using System.Collections.Generic;

  public class ConditionTemplate : WorkflowDefinition, IKind
  {
    public string Kind { get; set; }
    public IList<InputTemplate> Parameters { get; set; }
    public IList<InputTemplate> Inputs { get; set; }
    public IList<ConditionTemplate> Conditions { get; set; }
    public IDictionary<string, object> Variables { get; set; }
    public StepTemplate Step { get; set; }
    public string Expression { get; set; }
    public string Execution { get; set; }
    public bool Data { get; set; }
    public bool Disable { get; set; }
  }
}
