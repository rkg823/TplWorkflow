// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Stores
{
  using TplWorkflow.Extensions.Validations;
  using TplWorkflow.Stores.Interfaces;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class WorkflowMemoryStore : IWorkflowStore
  {
    private readonly List<(Core.WorkflowInstance workflow, IServiceProvider provider)> instances
      = new List<(Core.WorkflowInstance workflow, IServiceProvider provider)>();

    public (Core.WorkflowInstance workflow, IServiceProvider provider) FirstOrDefault(
      Func<(Core.WorkflowInstance workflow, IServiceProvider provider), bool> predicate)
    {
      predicate.Required(nameof(predicate));
      lock (instances)
      {
        return instances.FirstOrDefault(predicate);
      }
    }

    public IList<(Core.WorkflowInstance workflow, IServiceProvider provider)> Where(
      Func<(Core.WorkflowInstance workflow, IServiceProvider provider), bool> predicate)
    {
      predicate.Required(nameof(predicate));

      lock (instances)
      {
        return instances.Where(predicate).ToList();
      }
    }

    public (Core.WorkflowInstance workflow, IServiceProvider provider) Get(string name)
    {
      name.Required(nameof(name));

      lock (instances)
      {
        return instances.FirstOrDefault(e => e.workflow.Name == name);
      }
    }

    public IList<(Core.WorkflowInstance workflow, IServiceProvider provider)> Get()
    {
      lock (instances)
      {
        return instances.ToList();
      }
    }

    public (Core.WorkflowInstance workflow, IServiceProvider provider) Get(string name, int version)
    {
      name.Required(nameof(name));

      lock (instances)
      {
        return instances.FirstOrDefault(e => e.workflow.Name.ToLower() == name.ToLower() && e.workflow.Version == version);
      }
    }

    public bool Contains(string name, int version)
    {
      name.Required(nameof(name));

      lock (instances)
      {
        return instances.Exists(e => e.workflow.Name.ToLower() == name.ToLower() && e.workflow.Version == version);
      }
    }

    public bool Contains(string name)
    {
      name.Required(nameof(name));

      lock (instances)
      {
        return instances.Exists(e => e.workflow.Name.ToLower() == name.ToLower());
      }
    }

    public bool Any(Func<(Core.WorkflowInstance workflow, IServiceProvider provider), bool> predicate)
    {
      predicate.Required(nameof(predicate));

      lock (instances)
      {
        return instances.Any(predicate);
      }
    }

    public bool Add((Core.WorkflowInstance workflow, IServiceProvider provider) workFlow)
    {
      lock (instances)
      {
        instances.Add(workFlow);
      }

      return true;
    }

    public bool Clear()
    {
      lock (instances)
      {
        instances.Clear();
      }

      return true;
    }

    public bool Remove(string name, int version)
    {
      bool removed = false;

      name.Required(nameof(name));

      lock (instances)
      {
        var wf = instances.FirstOrDefault(e => e.workflow.Name.ToLowerInvariant() == name.ToLowerInvariant() && e.workflow.Version == version);

        if (wf.workflow == null)
        {
          return removed;
        }

        instances.Remove(wf);
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

      lock (instances)
      {
        var wf = instances.FirstOrDefault(
          e => e.workflow.Name.ToLowerInvariant() == tupple.workflow.Name.ToLowerInvariant() && e.workflow.Version == tupple.workflow.Version);

        if (wf.workflow != null)
        {
          instances.Remove(wf);
          update = true;
        }

        instances.Add(tupple);
      }

      return update;
    }
  }
}
