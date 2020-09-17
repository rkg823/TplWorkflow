// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Common;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Core.Maps;
using TplWorkflow.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Conditions
{
  public class ExpressionCondition: Condition
  {
    public ExpressionCondition(Delegate method, IList<Input> inputs, IList<Variable> variables)
    {
      Inputs = inputs;
      Method = method;
      Variables = variables;
    }

    public IList<Variable> Variables { get; }
    public IList<Map> Maps { get; }

    public IList<Input> Inputs { get; }
    public Delegate Method { get; }

    public override Task<AsyncResult<bool>> Resolve(ExecutionContext context)
    {
      var _context = context.ResolveMaps(Maps, Variables);
      var inputs = new List<object>();
        foreach (var _input in Inputs)
        {
          inputs.Add(_input.Resolve(_context));
        }
        var result = (bool) Method.DynamicInvoke(inputs.ToArray());
        return Task.FromResult(new AsyncResult<bool>(result));
    }
  }
}
