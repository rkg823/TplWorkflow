// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Services
{
  using TplWorkflow.Extensions.Validations;
  using TplWorkflow.Models;
  using TplWorkflow.Models.Templates;
  using Microsoft.Extensions.DependencyInjection;
  using Newtonsoft.Json;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public partial class WorkflowLoader
  {
    #region From Single Json 
    public WorkflowDefinition FromJson(string workflow)
    {
      return FromJson(workflow, default(ServiceCollection));
    }

    public WorkflowDefinition FromJson(string workflow, ServiceCollection services)
    {
      var template = JsonConvert.DeserializeObject<WorkflowTemplate>(workflow);
      var context = new TemplateContext();

      return Register(template, context, services);
    }

    public WorkflowDefinition FromJson(string workflow, Action<ServiceCollection> configureServices)
    {
      var sc = new ServiceCollection();
      configureServices(sc);

      return FromJson(workflow, sc);
    }

    public WorkflowDefinition FromJson(string workflow, IList<string> pipelines)
    {
      var _wftemplate = JsonConvert.DeserializeObject<WorkflowTemplate>(workflow);

      pipelines ??= new List<string>();

      _wftemplate.Pipelines = pipelines.Select(e =>
      {
        var pipe = JsonConvert.DeserializeObject<PipelineTemplate>(e);

        pipe.Name.Required("Link pipeline should have a name define.");
        pipe.Version.Required(1, "Link pipeline should have a version define.");

        return pipe;
      }).ToList();

      return Register(_wftemplate);
    }

    public WorkflowDefinition FromJson(string workflow, IList<string> pipelines, Action<ServiceCollection> configureServices)
    {
      configureServices.Required("Configure Services can not be null.");

      var _wftemplate = JsonConvert.DeserializeObject<WorkflowTemplate>(workflow);


      pipelines ??= new List<string>();

      _wftemplate.Pipelines = pipelines.Select(e => JsonConvert.DeserializeObject<PipelineTemplate>(e)).ToList();

      var sc = new ServiceCollection();
      configureServices(sc);

      var context = new TemplateContext();

      return Register(_wftemplate, context, sc);
    }

    public WorkflowDefinition FromJson(string workflow, IList<string> pipelines, IList<string> conditions)
    {
      pipelines ??= new List<string>();
      conditions ??= new List<string>();

      var _wftemplate = JsonConvert.DeserializeObject<WorkflowTemplate>(workflow);
      var _pipelines = pipelines.Select(e => JsonConvert.DeserializeObject<PipelineTemplate>(e)).ToList();
      var _conditions = conditions.Select(e => JsonConvert.DeserializeObject<ConditionTemplate>(e)).ToList();

      var context = new TemplateContext(_pipelines, _conditions);

      return Register(_wftemplate, context);
    }

    public WorkflowDefinition FromJson(string workflow, IList<string> pipelines, IList<string> conditions, Action<ServiceCollection> configureServices)
    {
      configureServices.Required("Configure Services can not be null.");

      pipelines ??= new List<string>();
      conditions ??= new List<string>();

      var _wftemplate = JsonConvert.DeserializeObject<WorkflowTemplate>(workflow);
      var _pipelines = pipelines.Select(e => JsonConvert.DeserializeObject<PipelineTemplate>(e)).ToList();
      var _conditions = conditions.Select(e => JsonConvert.DeserializeObject<ConditionTemplate>(e)).ToList();

      var sc = new ServiceCollection();
      configureServices(sc);
      var context = new TemplateContext(_pipelines, _conditions);

      return Register(_wftemplate, context, sc);
    }
    #endregion

    #region From List of Jsons
    public IList<WorkflowDefinition> FromJson(IList<string> workflows)
    {
      return workflows.Select(e => FromJson(e, default(ServiceCollection))).ToList();
    }

    public IList<WorkflowDefinition> FromJson(IList<(string workflow, ServiceCollection services)> jsons)
    {
      return jsons.Select((e) =>
      {
        var template = JsonConvert.DeserializeObject<WorkflowTemplate>(e.workflow);
        var context = new TemplateContext();

        return Register(template, context, e.services);
      }).ToList();
    }
    #endregion
  }
}
