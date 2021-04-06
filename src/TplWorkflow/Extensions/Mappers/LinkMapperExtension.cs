// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Extensions.Mappers
{
  using TplWorkflow.Extensions.Validations;
  using TplWorkflow.Models.Templates;
  using System.Linq;

  public static class LinkMapperExtension
  {
    public static PipelineTemplate MapLinkPipeline(this PipelineTemplate source, TemplateContext context)
    {
      if (source.NotLinkPipeline())
      {
        return source;
      }

      source.Name.Required("link pipeline must define a name.");
      source.Version.Required(1, "link pipeline must define a pipeline version.");

      var template = context.LinkedPipelines?.FirstOrDefault(e => e.Name == source.Name && e.Version == source.Version);

      template.Required($"Pipline not found with name {source.Name} and version {source.Version}");

      var _tempalte = template.NotLinkPipeline() ? template : template.MapLinkPipeline(context);

      return _tempalte;
    }

    public static ConditionTemplate MapLinkCondition(this ConditionTemplate source, TemplateContext context)
    {
      if (source.NotLinkCondition())
      {
        return source;
      }

      source.Name.Required("link condition must define a name.");
      source.Version.Required(1, "link condition must define a version.");

      var template = context.LinkedConditions?.FirstOrDefault(e => e.Name == source.Name && e.Version == source.Version);

      template.Required($"Condition not found with name {source.Name} and version {source.Version}");

      var _tempalte = template.NotLinkCondition() ? template : template.MapLinkCondition(context);
      return _tempalte;
    }
  }
}
