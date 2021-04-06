// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Test.Extensions.Mappers
{
  using TplWorkflow.Core.Common;
  using TplWorkflow.Core.Methods;
  using TplWorkflow.Core.Steps;
  using TplWorkflow.Exceptions;
  using TplWorkflow.Extensions.Mappers;
  using TplWorkflow.Models.Templates;
  using TplWorkflow.Stores;
  using TplWorkflow.Stores.Interfaces;
  using TplWorkflow.Test.Mock;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moq;
  using System;
  using System.Collections.Generic;

  [TestClass]
  public class StepMapperExtensionTest
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
    public void MapTaskStepWithoutKind()
    {
      var context = new TemplateContext();
      var template = new StepTemplate
      {
        Contract = typeof(IMockContract).AssemblyQualifiedName,
        Method = nameof(IMockContract.GetData),
        Inputs = new List<InputTemplate> {
          new InputTemplate {
            DataType = typeof(MockModel).AssemblyQualifiedName,
            Kind = DefinitionStore.StepInput
          }
        }
      };

      var step = template.Map(context) as AsyncStep;
    }

    [TestMethod]
    public void MapTaskStepWithInputTest()
    {
      var context = new TemplateContext();
      var template = new StepTemplate
      {
        Kind = DefinitionStore.TaskStep,
        Contract = typeof(IMockContract).AssemblyQualifiedName,
        Method = nameof(IMockContract.GetData),
        Inputs = new List<InputTemplate> {
          new InputTemplate {
            DataType = typeof(MockModel).AssemblyQualifiedName,
            Kind = DefinitionStore.StepInput
          }
        }
      };

      var step = template.Map(context) as AsyncStep;

      Assert.IsNull(step.Condition);
      Assert.IsNotNull(step.Method);
      Assert.IsNotNull(step.Method.Inputs);
      Assert.AreEqual(typeof(ContractMethod), step.Method.GetType());
      Assert.AreEqual(1,step.Method.Inputs.Count);
      Assert.AreEqual(typeof(MockModel), step.Method.Inputs[0].DataType);
    }

    [TestMethod]
    public void MapTaskStepWithConditionTest()
    {
      var context = new TemplateContext();
      var template = new StepTemplate
      {
        Kind = DefinitionStore.TaskStep,
        Contract = typeof(IMockContract).AssemblyQualifiedName,
        Method = nameof(IMockContract.GetData),
        Condition = new ConditionTemplate
        {
          Data = true,
          Kind = DefinitionStore.InlineCondition,
        }
      };

      var step = template.Map(context) as AsyncStep;
    
      Assert.IsNotNull(step.Method.Inputs);
      Assert.IsNotNull(step.Condition);
      Assert.IsNotNull(step.Method);
      Assert.IsNotNull(step.Condition);
      Assert.AreEqual(0, step.Method.Inputs.Count);
      Assert.AreEqual(typeof(ContractMethod), step.Method.GetType());
    }

    [TestMethod]
    public void MapTaskStepWithOutputTest()
    {
      var context = new TemplateContext();
      var template = new StepTemplate
      {
        Kind = DefinitionStore.TaskStep,
        Contract = typeof(IMockContract).AssemblyQualifiedName,
        Method = nameof(IMockContract.GetData),
       Outputs = new List<OutputTemplate>
       {
         new OutputTemplate
         {
           Kind = DefinitionStore.StepOutput,
           Name = "test",
           Scope = DefinitionStore.GlobalScope
         }
       }
      };

      var step = template.Map(context) as AsyncStep;

      Assert.IsNull(step.Condition);
      Assert.IsNotNull(step.Method);
      Assert.AreEqual(1, step.Outputs.Count);
      Assert.AreEqual(typeof(ContractMethod), step.Method.GetType());
    }

    [TestMethod]
    [ExpectedException(typeof(WorkflowException))]
    public void MapTaskStepWithNoScopeOutputTest()
    {
      var context = new TemplateContext();
      var template = new StepTemplate
      {
        Kind = DefinitionStore.TaskStep,
        Contract = typeof(IMockContract).AssemblyQualifiedName,
        Method = nameof(IMockContract.GetData),
        Outputs = new List<OutputTemplate>
       {
         new OutputTemplate
         {
           Kind = DefinitionStore.StepOutput,
           Name = "test"
         }
       }
      };

      var pstore = new Mock<IVariableStore>();
      var gstore = new Mock<IVariableStore>();

      pstore.Setup(e => e.Add(It.IsAny<string>(), It.IsAny<object>())).Returns(true);
      var ec = new ExecutionContext("some data", sp, gstore.Object, pstore.Object);

      var step = template.Map(context) as AsyncStep;
      var result = step.Resolve(ec);

      Assert.IsNotNull(result);
      Assert.AreEqual(true, result);
    }

    [TestMethod]
    [ExpectedException(typeof(WorkflowException))]
    public void PipelineStepWithoutKindTest() 
    {
      var context = new TemplateContext();
      var template = new StepTemplate
      {
        Kind = DefinitionStore.PipelineStep,
        Pipeline = new PipelineTemplate(),
      };

      var step = template.Map(context) as PipelineStep;

      Assert.IsNotNull(step.Pipeline);
    }

    [TestMethod]
    public void PipelineStepTest()
    {
      var context = new TemplateContext();
      var template = new StepTemplate
      {
        Kind = DefinitionStore.PipelineStep,
        Pipeline = new PipelineTemplate { 
          Kind= DefinitionStore.SequentialPipeline,
          Steps = new List<StepTemplate>()
        },
      };

      var step = template.Map(context) as PipelineStep;

      Assert.IsNotNull(step.Pipeline);
    }

    [TestMethod]
    public void WorkflowStepTest()
    {
      var context = new TemplateContext();
      var template = new StepTemplate
      {
        Kind = DefinitionStore.WorkflowStep,
        Name = "test",
        Version = 1      
      };

      var step = template.Map(context) as WorkflowStep;

      Assert.AreEqual(1,step.Version);
      Assert.AreEqual("test", step.Name);
    }
  }
}
