using TplWorkflow.Core.Common;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Inputs;
using System;
using System.Collections.Generic;

namespace TplWorkflow.Core.Steps
{
  public class ExpressionStep : Step
  {
    public IList<Input> Inputs { get; }
    public Delegate Method { get; }
    public ExpressionStep(string id, Condition condition, Delegate method,  IList<Input> inputs) : base(id, condition)
    {
      Inputs = inputs;
      Method = method;
    }

    public override AsyncResult<object> Resolve(ExecutionContext context)
    {
      var inputs = new List<object>();
      foreach (var _input in Inputs)
      {
        inputs.Add(_input.Resolve(context));
      }
      var result = Method.DynamicInvoke(inputs.ToArray());
      return new AsyncResult<object>(result);
    }
  }
}
