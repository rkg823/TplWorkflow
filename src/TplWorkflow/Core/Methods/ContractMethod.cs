using TplWorkflow.Core.Common;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Extensions.Validations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Methods
{
  public class ContractMethod : Method
  {
    public Type Contract { get; }
    Func<MethodInfo, object, IList<object>, Task> GetTask { get; }
    public ContractMethod(MethodInfo info, Type contract, IList<Input> inputs, Func<MethodInfo, object, IList<object>, Task> getTask) : base(info, inputs)
    {
      Contract = contract;
      GetTask = getTask;
    }
    public override Task Resolve(ExecutionContext context)
    {
      var inputs = new List<object>();
      if (Inputs != null)
      {
        foreach (var input in Inputs)
        {
          inputs.Add(input.Resolve(context));
        }
      }
      var instance = context.ServiceProvider.GetService(Contract);
      instance.Required($"Type (name - {Contract}) does not have an implementation registered in the dependency.");
      return GetTask(MethodInfo, instance, inputs);
    }
  }
}
