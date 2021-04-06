// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Pipelines
{
  using TplWorkflow.Core.Conditions;
  using TplWorkflow.Core.Common;
  using TplWorkflow.Core.Common.Interfaces;
  using TplWorkflow.Core.Maps;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public abstract class Pipeline : IConditional
  {
    public Condition Condition { get; }
    public IList<Variable> Variables { get; }
    public IList<Map> Maps { get; }

    protected Pipeline(Condition condition, IList<Variable> variables, IList<Map> maps)
    {
      Condition = condition;
      Variables = variables;
      Maps = maps;
    }

    public abstract Task<AsyncResult<object>> Resolve(ExecutionContext context);
  }
}
