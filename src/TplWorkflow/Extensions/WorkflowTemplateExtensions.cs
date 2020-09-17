// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Pipelines;
using TplWorkflow.Exceptions;
using TplWorkflow.Models.Templates;
using TplWorkflow.Models.Templates.Interfaces;
using TplWorkflow.Stores;
using TplWorkflow.Stores.Interfaces;
using System;

namespace TplWorkflow.Extensions
{
  public static class WorkflowTemplateExtensions
  {
    public static bool OfKind<T>(this IKind kind)
    {
      return DefinitionStore.Get(kind.Kind) == typeof(T);
    }

    public static bool IsGlobalScope(this IScope scope)
    {
      return scope.Scope == DefinitionStore.GlobalScope;
    }

    public static bool IsLocalScope(this IScope scope)
    {
      return scope.Scope == null || scope.Scope == DefinitionStore.LocalScope || scope.Scope == DefinitionStore.PipelineScope;
    }

    public static Func<ExecutionContext, IVariableStore> GetScopeResolver(this IScope template)
    {
      if (template.IsLocalScope())
      {
        return (context) => context.PipelineVariables;
      }
      if (template.IsGlobalScope())
      {
        return (context) => context.GlobalVariables;
      }
      throw new WorkflowException($"{template.Scope} scope not supported.");
    }

    public static bool HasMaps(this IMappable template)
    {
      return template?.Maps?.Count > 0;
    }

    public static bool HasVariables(this PipelineTemplate template)
    {
      return template != null && template.Variables != null && template.Variables.Count > 0;
    }

    public static bool OfForeachKind(this PipelineTemplate template)
    {
      return template.OfKind<ForEachPipeline>() || template.OfKind<ParallelForeachPipeline>();
    }

    public static bool NotLinkPipeline(this IKind template) 
    {
      return template.Kind != DefinitionStore.LinkPipeline;
    }

    public static bool NotLinkCondition(this IKind template)
    {
      return template.Kind != DefinitionStore.LinkCondition;
    }


  }
}

