// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Common;
using TplWorkflow.Core.Common.Interfaces;
using TplWorkflow.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Conditions
{
  public class OrCondition : Condition, IMultiCondition
  {
    public OrCondition(IList<Condition> conditions)
    {
      Conditions = conditions;
    }
    public IList<Condition> Conditions { get; }
    public override async Task<AsyncResult<bool>> Resolve(ExecutionContext context)
    {
      var evaluated = (await this.Evaluate(context)).Contains(true);
      return new AsyncResult<bool>(evaluated);
    }
  }
}
