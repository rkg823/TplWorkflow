// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Conditions
{
  using TplWorkflow.Core.Common;
  using TplWorkflow.Core.Maps;
  using TplWorkflow.Core.Steps;
  using TplWorkflow.Extensions;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public class StepCondition : Condition
  {
    public IList<Variable> Variables { get; }
    public IList<Map> Maps { get; }
    public Step Step { get; }

    public StepCondition(Step step, IList<Variable> variables)
    {
      Step = step;
      Variables = variables;
      Maps = new List<Map>();
    }

    public override async Task<AsyncResult<bool>> Resolve(ExecutionContext context)
    {
      var _context = context.ResolveMaps(Maps, Variables);
      var result = (bool)await Step.Resolve(_context).Value();
      return new AsyncResult<bool>(result);
    }
  }
}
