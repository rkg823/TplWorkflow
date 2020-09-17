// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Models.Templates.Interfaces;

namespace TplWorkflow.Models.Templates
{
  public abstract class TemplateKind: IKind
  {
    public abstract string Kind {get;set;}
  }
}
