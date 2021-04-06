// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Test
{
  using TplWorkflow.Core;
  using TplWorkflow.Core.Common;
  using TplWorkflow.Core.Conditions;
  using TplWorkflow.Core.Inputs;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moq;
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  [TestClass]
  public class ConditionTest
  {
    private ServiceProvider provider = null;
    [TestInitialize]
    public void Init()
    {
      provider = new ServiceCollection().BuildServiceProvider();

    }
    [TestMethod]
    public async Task ExpressionTrueConditionTest()
    {
      var context = new ExecutionContext(provider);
      var mockInput1 = new Mock<Input>(MockBehavior.Strict, new object[] { typeof(string) });
      mockInput1.Setup(e => e.Resolve(It.IsNotNull<ExecutionContext>())).Returns("some data");
      var mockInput2 = new Mock<Input>(MockBehavior.Strict, new object[] { typeof(string) });
      mockInput2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns("some data");
      var inputs = new List<Input> { mockInput1.Object, mockInput2.Object };
      Func<string, string, bool> del = (i1, i2) => i1 == i2;
      
      var variables = new List<Variable>();  
      var condition = new ExpressionCondition(del, inputs, variables);
      var result = await condition.Resolve(context);

      Assert.IsNotNull(condition.Method);
      Assert.IsNotNull(condition.Inputs);
      Assert.AreEqual(2, condition.Inputs.Count);
      Assert.AreEqual(true, await result.Value(), "The condition should resovle true value");
    }

    [TestMethod]
    public async Task ExpressionFalseConditionTest()
    {
      var context = new ExecutionContext(provider);
      var mockInput1 = new Mock<Input>(MockBehavior.Strict, new object[] { typeof(string) });
      mockInput1.Setup(e => e.Resolve(It.IsNotNull<ExecutionContext>())).Returns("some data1");
      var mockInput2 = new Mock<Input>(MockBehavior.Strict, new object[] { typeof(string) });
      mockInput2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns("some data2");
      var inputs = new List<Input> { mockInput1.Object, mockInput2.Object };
      Func<string, string, bool> del = (i1, i2) => i1 == i2;
      
      var variables = new List<Variable>();
      var condition = new ExpressionCondition(del, inputs, variables);
      var result = await condition.Resolve(context);
      
      Assert.IsNotNull(condition.Method);
      Assert.IsNotNull(condition.Inputs);
      Assert.AreEqual(2, condition.Inputs.Count);
      Assert.AreEqual(false, await result.Value(), "The condition should resovle false value");
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public async Task NullInputsExpressionConditionTest()
    {
      var context = new ExecutionContext(provider);
      Func<string, string, bool> del = (i1, i2) => i1 == i2;
      
      var variables = new List<Variable>();
      var condition = new ExpressionCondition(del, null, variables);
      var result = await condition.Resolve(context);
     
      Assert.IsNotNull(condition.Method);
      Assert.IsNull(condition.Inputs);
      Assert.AreEqual(2, condition.Inputs.Count);
      Assert.AreEqual(true, await result.Value(), "The condition should resovle true value");
    }

    [TestMethod]
    public async Task InlineTrueConditionTest()
    {
      var context = new ExecutionContext(provider);
      var condition = new InlineCondition(true);
      var result = await condition.Resolve(context);
      
      Assert.IsNotNull(condition.Data);
      Assert.AreEqual(true, await result.Value(), "The condition should resovle true value");
    }

    [TestMethod]
    public async Task InlineFalseConditionTest()
    {
      var context = new ExecutionContext(provider);
      var condtition = new InlineCondition(false);
      var result = await condtition.Resolve(context);
      
      Assert.IsNotNull(condtition.Data);
      Assert.AreEqual(false, await result.Value(), "The condition should resovle true value");
    }

  }
}
