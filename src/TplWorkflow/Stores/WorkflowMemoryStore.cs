using TplWorkflow.Extensions.Validations;
using TplWorkflow.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TplWorkflow.Stores
{
  public class WorkflowMemoryStore: IWorkflowStore
  {
    private readonly List<(Core.WorkflowInstance workflow, IServiceProvider provider)> _instances = new List<(Core.WorkflowInstance workflow, IServiceProvider provider)>();
    public (Core.WorkflowInstance workflow, IServiceProvider provider) FirstOrDefault(Func<(Core.WorkflowInstance workflow, IServiceProvider provider), bool> predicate)
    {
      predicate.Required(nameof(predicate));
      lock (_instances)
      {
        return _instances.FirstOrDefault(predicate);
      }
    }

    public IList<(Core.WorkflowInstance workflow, IServiceProvider provider)> Where(Func<(Core.WorkflowInstance workflow, IServiceProvider provider), bool> predicate)
    {
      predicate.Required(nameof(predicate));
      lock (_instances)
      {
        return _instances.Where(predicate).ToList();
      }
    }

    public (Core.WorkflowInstance workflow, IServiceProvider provider) Get(string name)
    {
      name.Required(nameof(name));
      lock (_instances)
      {
        return _instances.FirstOrDefault(e => e.workflow.Name == name);
      }
    }

    public IList<(Core.WorkflowInstance workflow, IServiceProvider provider)> Get()
    {
      lock (_instances)
      {
        return _instances.ToList();
      }
    }
    public (Core.WorkflowInstance workflow, IServiceProvider provider) Get(string name, int version)
    {
      name.Required(nameof(name));
      lock (_instances)
      {
        return _instances.FirstOrDefault(e => e.workflow.Name.ToLower() == name.ToLower() && e.workflow.Version == version);
      }
    }

    public bool Contains(string name, int version)
    {
      name.Required(nameof(name));
      lock (_instances)
      {
        return _instances.Exists(e => e.workflow.Name.ToLower() == name.ToLower() && e.workflow.Version == version);
      }
    }

    public bool Contains(string name)
    {
      name.Required(nameof(name));
      lock (_instances)
      {
        return _instances.Exists(e => e.workflow.Name.ToLower() == name.ToLower());
      }
    }

    public bool Any(Func<(Core.WorkflowInstance workflow, IServiceProvider provider), bool> predicate)
    {
      predicate.Required(nameof(predicate));
      lock (_instances)
      {
        return  _instances.Any(predicate);
      }
    }

    public bool Add((Core.WorkflowInstance workflow, IServiceProvider provider) workFlow)
    {
      lock (_instances)
      {
        _instances.Add(workFlow);
      }
      return true;
    }

    public bool Clear()
    {
      lock (_instances)
      {
        _instances.Clear();
      }
      return true;
    }

    public bool Remove(string name, int version)
    {
      bool removed = false;
      name.Required(nameof(name));
      lock (_instances)
      {
        var wf = _instances.FirstOrDefault(e => e.workflow.Name.ToLowerInvariant() == name.ToLowerInvariant() && e.workflow.Version == version);
        if (wf.workflow == null) return removed;
        _instances.Remove(wf);
        removed = true;
      }
      return removed;
    }

    public bool Update((Core.WorkflowInstance workflow, IServiceProvider provider) tupple)
    {
      tupple.workflow.Required(nameof(tupple.workflow));
      tupple.provider.Required(nameof(tupple.provider));
      tupple.workflow.Name.Required(nameof(tupple.workflow.Name));
      bool update = false;
      lock (_instances)
      {
        var wf = _instances.FirstOrDefault(e => e.workflow.Name.ToLowerInvariant() == tupple.workflow.Name.ToLowerInvariant() && e.workflow.Version == tupple.workflow.Version);
        if (wf.workflow != null)
        {
          _instances.Remove(wf);
          update = true;
        }
        _instances.Add(tupple);
      }
      return update;
    }
  }
}
