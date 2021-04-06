// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Common.Interfaces
{
  using TplWorkflow.Core.Conditions;

  public interface IConditional
  {
    Condition Condition { get; }
  }
}
