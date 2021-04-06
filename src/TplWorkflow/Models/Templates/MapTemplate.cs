// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Models.Templates
{
  using TplWorkflow.Models.Templates.Interfaces;

  public class MapTemplate: IKind
  {
    public string Kind { get; set; }
    public InputTemplate From { get; set; }
    public string To {get; set; }
  }
}
