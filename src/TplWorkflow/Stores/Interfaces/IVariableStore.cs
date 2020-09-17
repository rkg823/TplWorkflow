// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core;
using System.Collections.Generic;

namespace TplWorkflow.Stores.Interfaces
{
  public interface IVariableStore
  {
    bool Add(string key, object value);
    bool Add(Variable variable);
    object Get(string key);
    IList<Variable> Get();
    bool Contains(string key);
  }
}