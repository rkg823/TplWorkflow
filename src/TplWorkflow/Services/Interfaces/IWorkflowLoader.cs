// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TplWorkflow.Models;
using TplWorkflow.Models.Templates;

namespace TplWorkflow.Services.Interfaces
{
  public interface IWorkflowLoader
  {
    IList<WorkflowDefinition> FromJson(IList<(string workflow, ServiceCollection services)> jsons);
    IList<WorkflowDefinition> FromJson(IList<string> workflows);
    WorkflowDefinition FromJson(string workflow);
    WorkflowDefinition FromJson(string workflow, Action<ServiceCollection> configureServices);
    WorkflowDefinition FromJson(string workflow, IList<string> pipelines);
    WorkflowDefinition FromJson(string workflow, IList<string> pipelines, Action<ServiceCollection> configureServices);
    WorkflowDefinition FromJson(string workflow, IList<string> pipelines, IList<string> conditions);
    WorkflowDefinition FromJson(string workflow, IList<string> pipelines, IList<string> conditions, Action<ServiceCollection> configureServices);
    WorkflowDefinition FromJson(string workflow, ServiceCollection services);
    WorkflowDefinition Register(WorkflowTemplate template);
    WorkflowDefinition Register(WorkflowTemplate template, TemplateContext context);
    WorkflowDefinition Register(WorkflowTemplate template, TemplateContext context, IServiceCollection services);
  }
}