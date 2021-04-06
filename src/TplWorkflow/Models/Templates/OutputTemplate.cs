// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Models.Templates
{
  using TplWorkflow.Models.Templates.Interfaces;
  using System.Collections.Generic;

  public class OutputTemplate: IKind, IScope
  {
    public string Name { get; set; }
    public string Kind { get; set; }
    public string DataType { get; set; }
    public string Scope { get; set; }
    public IList<InputTemplate> Inputs { get; set; }
    public IList<InputTemplate> Parameters { get; set; }
    public string Expression { get; set; }
    public object Value { get; set; }
  }
}
