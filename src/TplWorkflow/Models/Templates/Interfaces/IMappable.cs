// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using System.Collections.Generic;

namespace TplWorkflow.Models.Templates.Interfaces
{
  public interface IMappable
  {
    IList<MapTemplate> Maps{ get; }
  }
}
