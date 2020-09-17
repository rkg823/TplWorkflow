// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Exceptions;
using TplWorkflow.Core.Pipelines;
using TplWorkflow.Models.Templates;
using TplWorkflow.Stores;
using TplWorkflow.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TplWorkflow.Extensions.Mappers;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace TplWorkflow.Test
{
  [TestClass]
  public class PipelineMapperExtensionTest
  {
    private IServiceProvider sp;
    [TestInitialize]
    public void Init()
    {
      var sc = new ServiceCollection();
      sp = sc.BuildServiceProvider();
    }

    [TestMethod]
    [ExpectedException(typeof(WorkflowException))]
    public void PipelineWithNoStepsTest()
    {
      var context = new TemplateContext();
      var template = new PipelineTemplate
      {
        Kind = DefinitionStore.SequentialPipeline,
        Steps = null
      };
      template.Map(context);
    }

    [TestMethod]
    [ExpectedException(typeof(WorkflowException))]
    public void PipelineWithNoKindTest()
    {
      var context = new TemplateContext();
      var template = new PipelineTemplate
      {
        Steps = null
      };
      template.Map(context);
    }

    [TestMethod]
    public void ParallelPipelineTest()
    {
      var context = new TemplateContext();
      var template = new PipelineTemplate
      {
        Kind = DefinitionStore.ParallelPipeline,
        Steps = new List<StepTemplate>
        {
          new StepTemplate
          {
            Kind = DefinitionStore.TaskStep,
            Method = "GetData",
            Contract = typeof(IMockContract).AssemblyQualifiedName,
          }
        }
      };
      var pipeline = template.Map(context) as ParallelPipeline;
      Assert.IsNotNull(pipeline.Steps);
      Assert.IsNull(pipeline.Condition);
      Assert.AreEqual(1,pipeline.Steps.Count);
    }

    [TestMethod]
    public void ParallelPipelineWithConditionTest()
    {
      var context = new TemplateContext();
      var template = new PipelineTemplate
      {
        Kind = DefinitionStore.ParallelPipeline,
        Condition = new ConditionTemplate
        {
          Kind = DefinitionStore.InlineCondition,
          Data = true
        },
        Steps = new List<StepTemplate>
        {
          new StepTemplate
          {
            Kind = DefinitionStore.TaskStep,
            Method = "GetData",
            Contract = typeof(IMockContract).AssemblyQualifiedName,
          }
        }
      };
      var pipeline = template.Map(context) as ParallelPipeline;
      Assert.IsNotNull(pipeline.Steps);
      Assert.IsNotNull(pipeline.Condition);
      Assert.AreEqual(1, pipeline.Steps.Count);
    }

    [TestMethod]
    public void SequentialPipelineTest()
    {
      var context = new TemplateContext();
      var template = new PipelineTemplate
      {
        Kind = DefinitionStore.SequentialPipeline,
        Steps = new List<StepTemplate>
        {
          new StepTemplate
          {
            Kind = DefinitionStore.TaskStep,
            Method = "GetData",
            Contract = typeof(IMockContract).AssemblyQualifiedName,
          }
        }
      };
      var pipeline = template.Map(context) as SequentialPipeline;
      Assert.IsNotNull(pipeline.Steps);
      Assert.IsNull(pipeline.Condition);
      Assert.AreEqual(1, pipeline.Steps.Count);
    }

    [TestMethod]
    public void SequentialPipelineWithConditionTest()
    {
      var context = new TemplateContext();
      var template = new PipelineTemplate
      {
        Kind = DefinitionStore.SequentialPipeline,
        Condition = new ConditionTemplate
        {
          Kind = DefinitionStore.InlineCondition,
          Data = true
        },
        Steps = new List<StepTemplate>
        {
          new StepTemplate
          {
            Kind = DefinitionStore.TaskStep,
            Method = "GetData",
            Contract = typeof(IMockContract).AssemblyQualifiedName,
          }
        }
      };
      var pipeline = template.Map(context) as SequentialPipeline;
      Assert.IsNotNull(pipeline.Steps);
      Assert.IsNotNull(pipeline.Condition);
      Assert.AreEqual(1, pipeline.Steps.Count);
    }

  }
}
