using TplWorkflow.Models.Templates.Interfaces;

namespace TplWorkflow.Models.Templates
{
  public class MapTemplate: IKind
  {
    public string Kind { get; set; }
    public InputTemplate From { get; set; }
    public string To {get; set; }
  }
}
