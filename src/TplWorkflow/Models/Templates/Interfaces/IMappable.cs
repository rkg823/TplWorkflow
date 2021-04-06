// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Models.Templates.Interfaces
{
  using System.Collections.Generic;

  public interface IMappable
  {
    IList<MapTemplate> Maps{ get; }
  }
}
