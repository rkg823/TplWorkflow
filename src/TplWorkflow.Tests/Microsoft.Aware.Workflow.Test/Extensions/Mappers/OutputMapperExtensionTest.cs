// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Exceptions;
using TplWorkflow.Extensions.Mappers;
using TplWorkflow.Core.Outputs;
using TplWorkflow.Models.Templates;
using TplWorkflow.Stores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TplWorkflow.Stores.Interfaces;
using Moq;
using TplWorkflow.Core.Common;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace TplWorkflow.Test
{
  [TestClass]
  public class OutputMapperExtensionTest
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
    public void OutputWithNoKindTest()
    {
      var template = new OutputTemplate
      {
        Name = "test",
        Scope = DefinitionStore.GlobalScope
      };
      template.Map();
    }
    [TestMethod]
    public void OutputWithNoScopeTest()
    {
      var template = new OutputTemplate
      {
        Name = "test",
        Kind = DefinitionStore.StepOutput,
      };
      var output = template.Map() as StepOutput;
      var pstore = new Mock<IVariableStore>();
      var gstore = new Mock<IVariableStore>();
      pstore.Setup(e => e.Add(It.IsAny<string>(), It.IsAny<object>())).Returns(true);
      var context = new ExecutionContext("some data", sp, gstore.Object, pstore.Object);

      var resolved = output.ResolveVariables(context);
      var result = output.Resolve(context);
      Assert.IsNotNull(result);
      Assert.AreEqual(true, result);
    }


    [TestMethod]
    public void StepOutputWithGlobalScopeTest()
    {
      var template = new OutputTemplate
      {
        Name = "test",
        Kind = DefinitionStore.StepOutput,
        Scope = DefinitionStore.GlobalScope
      };
      var output = template.Map() as StepOutput;
      Assert.IsNotNull(output.Name);
      Assert.IsNotNull(output.ResolveVariables);
      Assert.AreEqual("test", output.Name);
    }

    [TestMethod]
    public void StepOutputWithPipelineScopeTest()
    {
      var template = new OutputTemplate
      {
        Name = "test",
        Kind = DefinitionStore.StepOutput,
        Scope = DefinitionStore.LocalScope
      };
      var output = template.Map() as StepOutput;
      Assert.IsNotNull(output.Name);
      Assert.IsNotNull(output.ResolveVariables);
      Assert.AreEqual("test", output.Name);

    }

    [TestMethod]
    public void ExpressionOutputWithGlobalScopeTest()
    {
      var template = new OutputTemplate
      {
        Name = "test",
        Kind = DefinitionStore.ExpressionOutput,
        Scope = DefinitionStore.GlobalScope,
        Expression = "data",
        Parameters = new List<InputTemplate>
        {
          new InputTemplate
          {
            DataType = typeof(string).AssemblyQualifiedName,
            Name = "data",
            Kind = DefinitionStore.StepInput
          }
        }
      };
      var output = template.Map() as ExpressionOutput;
      Assert.IsNotNull(output.Name);
      Assert.IsNotNull(output.ResolveVariables);
      Assert.IsNotNull(output.Method);
      Assert.IsNotNull(output.Inputs);
      Assert.AreEqual(1, output.Inputs.Count);
      Assert.AreEqual("test", output.Name);
    }

    [TestMethod]
    public void ExpressionOutputWithPipelineScopeTest()
    {
      var template = new OutputTemplate
      {
        Name = "test",
        Kind = DefinitionStore.ExpressionOutput,
        Scope = DefinitionStore.LocalScope,
        Expression = "data",
        Parameters = new List<InputTemplate>
        {
          new InputTemplate
          {
            DataType = typeof(string).AssemblyQualifiedName,
            Name = "data",
            Kind = DefinitionStore.StepInput
          }
        }
      };
      var output = template.Map() as ExpressionOutput;
      Assert.IsNotNull(output.Name);
      Assert.IsNotNull(output.ResolveVariables);
      Assert.IsNotNull(output.Method);
      Assert.IsNotNull(output.Inputs);
      Assert.AreEqual(1, output.Inputs.Count);
      Assert.AreEqual("test", output.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(WorkflowException))]
    public void ExpressionOutputWithWrongParameterKindTest()
    {
      var template = new OutputTemplate
      {
        Name = "test",
        Kind = DefinitionStore.ExpressionOutput,
        Scope = DefinitionStore.LocalScope,
        Expression = "data",
        Parameters = new List<InputTemplate>
        {
          new InputTemplate
          {
            DataType = typeof(string).AssemblyQualifiedName,
            Name = "data",
            Kind = DefinitionStore.StepOutput
          }
        }
      };
      var output = template.Map() as ExpressionOutput;
      Assert.IsNotNull(output.Name);
      Assert.IsNotNull(output.ResolveVariables);
      Assert.IsNotNull(output.Method);
      Assert.IsNotNull(output.Inputs);
      Assert.AreEqual(1, output.Inputs.Count);
      Assert.AreEqual("test", output.Name);
    }
  }
}

