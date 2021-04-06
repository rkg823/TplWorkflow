// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Models.Templates
{
  using TplWorkflow.Models.Templates.Interfaces;
  using System.Collections.Generic;

  public class StepTemplate: IKind, IMappable
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public int? Version { get; set; }
    public string Kind { get; set; }
    public string Contract { get; set; }
    public string Instance { get; set; }
    public string Static { get; set; }
    public string Method { get; set; }
    public string Expression { get; set; }
    public object Template { get; set; }
    public ConditionTemplate Condition { get; set; }
    public IList<InputTemplate> Inputs { get; set; }
    public IList<MapTemplate> Maps { get; set; }
    public IList<OutputTemplate> Outputs { get; set; }
    public PipelineTemplate Pipeline { get; set; }
    public bool Disable { get; set; }

    public StepTemplate()
    {
      Inputs = new List<InputTemplate>();
      Outputs = new List<OutputTemplate>();
      Maps = new List<MapTemplate>();
    }
  }
}
