using TplWorkflow.Exceptions;
using TplWorkflow.Extensions.Mappers;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Models.Templates;
using TplWorkflow.Stores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TplWorkflow.Core.Common;
using System;
using Microsoft.Extensions.DependencyInjection;
using TplWorkflow.Stores.Interfaces;
using Moq;

namespace TplWorkflow.Test
{
  [TestClass]
  public class InputMapperExtensionTest
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
    public void InputWithNoKindTest()
    {
      var template = new InputTemplate
      {
        DataType = typeof(string).AssemblyQualifiedName,
        Name = "test",
      };
      template.Map();
    }

    [TestMethod]
    public void ExpressionInputTest()
    {
      var template = new InputTemplate
      {
        DataType = typeof(string).AssemblyQualifiedName,
        Expression = "data + \"1\"",
        Kind = DefinitionStore.ExpressionInput,
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
      var input = template.Map() as ExpressionInput;
      var result = input.Method.DynamicInvoke(new[] { "test" });
      Assert.IsNotNull(input.Inputs);
      Assert.IsNotNull(input.Method);
      Assert.IsNotNull(input.DataType);
      Assert.AreEqual(1, input.Inputs.Count);
      Assert.AreEqual(typeof(string), input.DataType);
      Assert.AreEqual("test1", result);
    }

    [TestMethod]
    public void InlineInputTest()
    {
      var template = new InputTemplate
      {
        DataType = typeof(string).AssemblyQualifiedName,
        Data = "some data",
        Kind = DefinitionStore.InlineInput
      };
      var input = template.Map() as InlineInput;
      Assert.IsNotNull(input.DataType);
      Assert.IsNotNull(input.Data);
      Assert.AreEqual(typeof(string), input.DataType);
      Assert.AreEqual("some data", input.Data);
    }

    [TestMethod]
    public void StepInputTest()
    {
      var template = new InputTemplate
      {
        Kind = DefinitionStore.StepInput,
        DataType = typeof(string).AssemblyQualifiedName,
      };
      var input = template.Map() as StepInput;
      Assert.IsNotNull(input.DataType);
      Assert.AreEqual(typeof(string), input.DataType);
    }

    [TestMethod]
    public void VariableInputWithGlobalScopeTest()
    {
      var template = new InputTemplate
      {
        DataType = typeof(string).AssemblyQualifiedName,
        Name = "test",
        Kind = DefinitionStore.VariableInput,
        Scope = DefinitionStore.GlobalScope
      };
      var input = template.Map() as VariableInput;
      Assert.IsNotNull(input.Name);
      Assert.IsNotNull(input.DataType);
      Assert.IsNotNull(input.ResolveVariables);
      Assert.AreEqual("test", input.Name);
      Assert.AreEqual(typeof(string), input.DataType);
    }

    [TestMethod]
    public void VariableInputWithPipelineScopeTest()
    {
      var template = new InputTemplate
      {
        DataType = typeof(string).AssemblyQualifiedName,
        Name = "test",
        Kind = DefinitionStore.VariableInput,
        Scope = DefinitionStore.LocalScope
      };
      var input = template.Map() as VariableInput;
      Assert.IsNotNull(input.Name);
      Assert.IsNotNull(input.DataType);
      Assert.IsNotNull(input.ResolveVariables);
      Assert.AreEqual("test", input.Name);
      Assert.AreEqual(typeof(string), input.DataType);
    }

    [TestMethod]
    public void VariableInputWithNoScopeTest()
    {
      var data = "result data";
      var template = new InputTemplate
      {
        DataType = typeof(string).AssemblyQualifiedName,
        Name = "test",
        Kind = DefinitionStore.VariableInput,
      };
      var input = template.Map() as VariableInput;
      var pstore = new Mock<IVariableStore>();
      var gstore = new Mock<IVariableStore>();
      pstore.Setup(e => e.Get(It.IsAny<string>())).Returns(data);
      var context = new ExecutionContext("some data",sp,gstore.Object,pstore.Object);
      var result =  input.Resolve(context);
      Assert.IsNotNull(result);
      Assert.AreEqual(data, result);
    }
  }
}
