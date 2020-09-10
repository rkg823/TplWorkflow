using TplWorkflow.Core;
using TplWorkflow.Stores.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TplWorkflow.Stores
{
  public class VariableMemoryStore : IVariableStore
  {
    private readonly ConcurrentDictionary<string, object> store;
    public VariableMemoryStore()
    {
      store = new ConcurrentDictionary<string, object>();
    }
    public bool Add(string key, object value)
    {
      return store.TryAdd(key, value);
    }

    public bool Add(Variable variable)
    {
      return store.TryAdd(variable.Name, variable.Data);
    }

    public object Get(string key)
    {
      var item = store.FirstOrDefault(e => e.Key == key);
      return item.Value;
    }
    public IList<Variable> Get()
    {
      return store.Select(e=>new Variable(e.Key,e.Value)).ToList();
    }


    public bool Contains(string key)
    {
      return store.ContainsKey(key);
    }
  }
}
