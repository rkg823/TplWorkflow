// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Models;
using TplWorkflow.Models.Templates;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TplWorkflow.Services.Interfaces
{
  public interface IWorklowLoader
  {
    WorkflowDefinition FromJson(string workflow);
    WorkflowDefinition FromJson(string workflow, IList<string> pipelines);
    WorkflowDefinition FromJson(string workflow, Action<ServiceCollection> configureServices);
    WorkflowDefinition FromJson(string workflow, ServiceCollection services);
    WorkflowDefinition FromJson(string workflow, IList<string> pipelines, Action<ServiceCollection> configureServices);
    WorkflowDefinition FromJson(string workflow, IList<string> pipelines,
    IList<string> conditions, Action<ServiceCollection> configureServices);
    IList<WorkflowDefinition> FromJson(IList<string> workflows);
    IList<WorkflowDefinition> FromJson(IList<(string workflow, ServiceCollection services)> jsons);
    WorkflowDefinition From(WorkflowTemplate template);
    WorkflowDefinition From(WorkflowTemplate template, Action<ServiceCollection> configureServices);
    WorkflowDefinition From(Func<WorkflowTemplate> factory, Action<ServiceCollection> configureServices);
  }
}
