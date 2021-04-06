// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Steps
{
  using TplWorkflow.Core.Conditions;
  using TplWorkflow.Core.Common;
  using TplWorkflow.Core.Common.Interfaces;

  public abstract class Step : IConditional
  {
    public string Id { get; }
    public Condition Condition { get; }
    public abstract AsyncResult<object> Resolve(ExecutionContext context);

    protected Step(string id, Condition condition)
    {
      Id = id;
      Condition = condition;
    }
  }
}
