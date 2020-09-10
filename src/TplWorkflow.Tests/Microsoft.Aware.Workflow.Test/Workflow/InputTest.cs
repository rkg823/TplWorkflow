using TplWorkflow.Core;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Stores;
using TplWorkflow.Stores.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace TplWorkflow.Test
{
  [TestClass]
  public class InputTest
  {
    private ServiceProvider provider = null;
    [TestInitialize]
    public void Init()
    {
      var sc = new ServiceCollection();
      sc.AddTransient<IVariableStore, VariableMemoryStore>();
      provider = sc.BuildServiceProvider();
    }
    [TestMethod]
    public void ExpressionInputTest()
    {
      var i1 = "data1";
      var i2 = "data2";
      var context = new ExecutionContext(provider);
      var mockInput1 = new Mock<Input>(MockBehavior.Strict, new object[] { typeof(string) });
      mockInput1.Setup(e => e.Resolve(It.IsNotNull<ExecutionContext>())).Returns(i1);
      var mockInput2 = new Mock<Input>(MockBehavior.Strict, new object[] { typeof(string) });
      mockInput2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(i2);
      var inputs = new List<Input> { mockInput1.Object, mockInput2.Object };
      Func<string, string, string> del = (i1, i2) => i1 + i2;
      var expInput = new ExpressionInput(typeof(string), del, inputs);
      var result = expInput.Resolve(context);
      Assert.IsNotNull(expInput.Method);
      Assert.IsNotNull(expInput.Inputs);
      Assert.IsNotNull(expInput.DataType);
      Assert.AreEqual(2,expInput.Inputs.Count);
      Assert.AreEqual(i1+i2, result, "The condition resovle method returns incorrect value");
    }

    [TestMethod]
    public void InlineInputTest()
    {
      var i1 = "data1";
      var context = new ExecutionContext(provider);
      var inlineInput = new InlineInput(typeof(string), i1);
      var result =inlineInput.Resolve(context);
      Assert.IsNotNull(inlineInput.Data);
      Assert.IsNotNull(inlineInput.DataType);
      Assert.AreEqual(i1, result, "The input resovle method returns incorrect value");
    }

    [TestMethod]
    public void StepInputTest()
    {
      var i1 = "data1";
      var context = new ExecutionContext(i1,provider);
      var inlineInput = new StepInput(typeof(string));
      var result = inlineInput.Resolve(context);
      Assert.IsNotNull(inlineInput.DataType);
      Assert.AreEqual(i1, result, "The input resovle method returns incorrect value");
    }

    [TestMethod]
    public void VariableInputGlobalSopeTest()
    {
      var i1 = "data1";
      var variables = new List<Variable>();
      variables.Add(new Variable("test", i1));
      var context = new ExecutionContext(null,provider, variables);
      Func<ExecutionContext, IVariableStore> resolver = (context) => context.GlobalVariables;
      var input = new VariableInput("test", typeof(string), resolver);
      var result = input.Resolve(context);
      Assert.IsNotNull(input.Name);
      Assert.IsNotNull(input.DataType);
      Assert.AreEqual("test",input.Name);
      Assert.AreEqual(i1, result, "The input resovle method returns incorrect value");
    }

    [TestMethod]
    public void VariableInputPipelineScopeTest()
    {
      var i1 = "data1";
      var mockStore = new Mock<IVariableStore>();
      mockStore.Setup(e => e.Get(It.IsAny<string>())).Returns(i1);
      var context = new ExecutionContext(null, provider, null, mockStore.Object);
      Func<ExecutionContext, IVariableStore> resolver = (context) => context.PipelineVariables;
      var input = new VariableInput("test", typeof(string), resolver);
      var result = input.Resolve(context);
      Assert.IsNotNull(input.Name);
      Assert.IsNotNull(input.DataType);
      Assert.AreEqual("test", input.Name);
      Assert.AreEqual(i1, result, "The input resovle method returns incorrect value");
    }
  }
}
