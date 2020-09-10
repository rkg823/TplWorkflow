using TplWorkflow.Models.Templates.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TplWorkflow.Models.Templates
{
  public class StepTemplate: IKind, IMappable
  {
    public StepTemplate()
    {
      Inputs = new List<InputTemplate>();
      Outputs = new List<OutputTemplate>();
      Maps = new List<MapTemplate>();
    }
    public string Id { get; set;}
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
  }
}
