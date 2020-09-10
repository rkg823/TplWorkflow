using TplWorkflow.Core.Pipelines;
using System.Collections.Generic;

namespace TplWorkflow.Core
{
  public class WorkflowInstance
  {
    public WorkflowInstance(string name, int version, string description, Pipeline pipeline, IList<Variable> variables)
    {
      Name = name;
      Version = version;
      Description = description;
      Pipeline = pipeline;
      Variables = variables;
    }
    public string Name { get; }
    public int Version { get; }
    public string Description { get; }
    public Pipeline Pipeline { get; }
    public IList<Variable> Variables { get; }
  }
}
