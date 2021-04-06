// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Conditions
{
  using TplWorkflow.Core.Common;
  using System.Threading.Tasks;

  public abstract class Condition
  {
    public abstract Task<AsyncResult<bool>> Resolve(ExecutionContext context);
  }
}
