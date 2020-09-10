using System.Collections.Generic;

namespace TplWorkflow.Models.Templates.Interfaces
{
  public interface IMappable
  {
    IList<MapTemplate> Maps{ get; }
  }
}
