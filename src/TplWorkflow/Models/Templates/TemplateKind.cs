// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Models.Templates
{
  using TplWorkflow.Models.Templates.Interfaces;

  public abstract class TemplateKind: IKind
  {
    public abstract string Kind {get;set;}
  }
}
