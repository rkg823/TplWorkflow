using TplWorkflow.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TplWorkflow.Services.Interfaces
{
  public interface IWorkflowHost
  {
    IList<WorkflowDefinition> Get();
    WorkflowDefinition Get(string name, int version);
    bool Contains(string name, int version);
    bool Clear();
    bool Remove(string name, int version);
    Task<object> StartAsync(string name, int version);
    Task<object> StartAsync(string name, int version, object data);
  }
}