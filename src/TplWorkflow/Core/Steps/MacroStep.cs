// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Common;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Extensions;
using System;
using System.Collections.Generic;

namespace TplWorkflow.Core.Steps
{
  public class MacroStep : Step
  {
    public string Expression { get; }
    IDictionary<string, (IList<Input> inputs, Delegate method)> Predicates { get; }
    public IDictionary<string, ExpressionStep> Expressions { get;}
    public MacroStep(string id, string expression, Condition condition,
      IDictionary<string, (IList<Input> inputs, Delegate method)> predicates) : base(id, condition)
    {
      Expression = expression;
      Predicates = predicates;
    }

    public override AsyncResult<object> Resolve(ExecutionContext context)
    {
      var exp = Expression;
      foreach(var p in Predicates)
      {
        var _inputs = p.Value.inputs.ResolveInputs(context);
        var vaule = p.Value.method.DynamicInvoke(_inputs);
        exp = exp.Replace(p.Key, vaule.ToString());
      }
      return new AsyncResult<object>(exp);
    }
  }
}
