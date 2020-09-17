// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Core.Outputs;
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
  public class OutputTest
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
    public void ExpressionOutputTest()
    {
      var i1 = "data1";
      var i2 = "data2";
      var variables = new List<Variable>();
      var context = new ExecutionContext(null, provider, variables);
      var mockInput1 = new Mock<Input>(MockBehavior.Strict, new object[] { typeof(string) });
      mockInput1.Setup(e => e.Resolve(It.IsNotNull<ExecutionContext>())).Returns(i1);
      var mockInput2 = new Mock<Input>(MockBehavior.Strict, new object[] { typeof(string) });
      mockInput2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(i2);
      var inputs = new List<Input> { mockInput1.Object, mockInput2.Object };
      Func<string, string, string> del = (i1, i2) => i1 + i2;
      Func<ExecutionContext, IVariableStore> resolver = (context) => context.GlobalVariables;
      var expOutput = new ExpressionOutput("test", del, inputs, resolver);
      var result = expOutput.Resolve(context);
      Assert.IsNotNull(expOutput.Method);
      Assert.IsNotNull(expOutput.Inputs);
      Assert.IsNotNull(expOutput.Name);
      Assert.AreEqual(2, expOutput.Inputs.Count);
      Assert.AreEqual(1, context.GlobalVariables.Get().Count);
      Assert.AreEqual(true, context.GlobalVariables.Contains("test"));
      Assert.AreEqual(i1+i2, context.GlobalVariables.Get("test"));
      Assert.AreEqual(true, result, "The Output resovle method returns incorrect value");
    }

    [TestMethod]
    public void StepOutputGlobalScopeTest() 
    {
      var i1 = "data1";
      var variables = new List<Variable>();
      var context = new ExecutionContext(i1, provider,variables);
      Func<ExecutionContext, IVariableStore> resolver = (c) => c.GlobalVariables;
      var inlineOutput = new StepOutput("test", resolver);
      var result = inlineOutput.Resolve(context);
      Assert.IsNotNull(inlineOutput.Name);
      Assert.AreEqual(1, context.GlobalVariables.Get().Count);
      Assert.AreEqual(true, context.GlobalVariables.Contains("test"));
      Assert.AreEqual(i1, context.GlobalVariables.Get("test"));
      Assert.AreEqual(true, result, "The Output resovle method returns incorrect value");
    }

    [TestMethod]
    public void StepOutputPipelineScopeTest()
    {
      var i1 = "data1";
      var mockStore = new Mock<IVariableStore>();
      mockStore.Setup(e => e.Get(It.IsAny<string>())).Returns(i1);
      mockStore.Setup(e => e.Add(It.IsAny<string>(), It.IsAny<object>())).Returns(true);
      var context = new ExecutionContext(i1, provider, null,mockStore.Object);
      Func<ExecutionContext, IVariableStore> resolver = (context) => context.PipelineVariables;
      var inlineOutput = new StepOutput("test", resolver);
      var result = inlineOutput.Resolve(context);
      Assert.IsNotNull(inlineOutput.Name);
      Assert.AreEqual(true, result, "The Output resovle method returns incorrect value");
    }
  }
}
