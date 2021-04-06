// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Conditions
{
  using TplWorkflow.Core.Common;
  using System.Threading.Tasks;

  public class InlineCondition : Condition
  {
    public bool Data { get; }

    public InlineCondition(bool data)
    {
      Data = data;
    }

    public override Task<AsyncResult<bool>> Resolve(ExecutionContext context)
    {
      return Task.FromResult(new AsyncResult<bool>(Data));
    }
  }
}
