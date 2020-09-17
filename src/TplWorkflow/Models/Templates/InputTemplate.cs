// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Models.Templates.Interfaces;
using System.Collections.Generic;

namespace TplWorkflow.Models.Templates
{
  public class InputTemplate: IKind, IScope
  {
    public string Kind { get; set; }
    public string Scope { get; set; }
    public string Data { get; set; }
    public string Name { get; set; }
    public string DataType { get; set; }
    public IList<InputTemplate> Inputs { get; set; }
    public IList<InputTemplate> Parameters { get; set; }
    public string Expression { get; set; }
    public bool Generic { get; set; }
  }
}
