using TplWorkflow.Core.Common;
using System;
using System.Collections.Generic;

namespace TplWorkflow.Core.Inputs
{
  public class ExpressionInput: Input
  {
    public ExpressionInput(Type type, Delegate method, IList<Input> inputs) : base(type)
    {
      Inputs = inputs;
      Method = method;
    }
    public IList<Input> Inputs { get; }
    public Delegate Method { get; }

    public override object Resolve(ExecutionContext context)
    {
      var inputs = new List<object>();
      foreach (var _input in Inputs)
      {
        inputs.Add(_input.Resolve(context));
      }
      var result = Method.DynamicInvoke(inputs.ToArray());
      return result;
    }
  }
}
