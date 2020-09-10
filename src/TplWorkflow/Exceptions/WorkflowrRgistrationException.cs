using System;

namespace TplWorkflow.Exceptions
{
  public class WorkflowrRgistrationException : Exception
  {
    public WorkflowrRgistrationException(string name)
        : base($"Workflow Name: {name} is not registered.")
    {
    }
    public WorkflowrRgistrationException(string name, int version)
      : base($"Workflow Name: {name} and Version: {version} is not registered.")
    {
    }
  }
}
