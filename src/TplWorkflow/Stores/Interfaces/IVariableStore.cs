// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Stores.Interfaces
{
  using TplWorkflow.Core;
  using System.Collections.Generic;

  public interface IVariableStore
  {
    bool Add(string key, object value);
    bool Add(Variable variable);
    object Get(string key);
    IList<Variable> Get();
    bool Contains(string key);
  }
}