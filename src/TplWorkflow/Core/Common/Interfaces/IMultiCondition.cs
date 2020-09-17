// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
using TplWorkflow.Core.Conditions;
using System.Collections.Generic;

namespace TplWorkflow.Core.Common.Interfaces
{
  public interface IMultiCondition
  {
    IList<Condition> Conditions { get; }
  }
}
