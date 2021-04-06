// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Common.Interfaces
{
  using TplWorkflow.Core.Conditions;
  using System.Collections.Generic;

  public interface IMultiCondition
  {
    IList<Condition> Conditions { get; }
  }
}
