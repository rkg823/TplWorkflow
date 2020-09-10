using TplWorkflow.Core;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Core.Methods;
using TplWorkflow.Core.Outputs;
using TplWorkflow.Core.Pipelines;
using TplWorkflow.Core.Steps;
using TplWorkflow.Stores;
using TplWorkflow.Stores.Interfaces;
using TplWorkflow.Test.Mock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TplWorkflow.Test
{
  [TestClass]
  public class StepTest
  {
    private ServiceProvider provider = null;
    [TestInitialize]
    public void Init()
    {
      var sc  = new ServiceCollection();
      sc.AddSingleton<IMockContract, MockService>();
      sc.AddTransient<IVariableStore, VariableMemoryStore>();
      provider = sc.BuildServiceProvider();
    }

    [TestMethod]
    public async Task PipelineStepTest()
    {
        object data = "some data";
        var exeResult = new AsyncResult<object>(data);
        var pipeline = new Mock<Pipeline>(MockBehavior.Strict, new object[] { null, null, null });
        pipeline.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(Task.FromResult(exeResult));
        var step = new PipelineStep("id", pipeline.Object, null);
        var context = new ExecutionContext("some value", provider);
        var er = step.Resolve(context);
        var result = await er.Value();
        Assert.IsNotNull(step.Id);
        Assert.IsNull(step.Condition);
        Assert.IsNotNull(step.Pipeline);
        Assert.AreEqual(data, result);
    }

    [TestMethod]
    public async Task PipelineStepWithConditionTrueTest()
    {
      object data = "some data";
      var exeResult = new AsyncResult<object>(data);
      var pipeline = new Mock<Pipeline>(MockBehavior.Strict, new object[] { null, null,null });
      pipeline.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(Task.FromResult(exeResult));
      var condition = new Mock<Condition>();
      var context = new ExecutionContext("some value", provider);
      condition.Setup(e => e.Resolve(context)).ReturnsAsync(new AsyncResult<bool>(true));
      var step = new PipelineStep("id", pipeline.Object, condition.Object);    
      var er = step.Resolve(context);
      var result = await er.Value();
      Assert.IsNotNull(step.Id);
      Assert.IsNotNull(step.Condition);
      Assert.IsNotNull(step.Pipeline);
      Assert.AreEqual(data, result);
    }

    [TestMethod]
    public async Task PipelineStepWithConditionFalseTest()
    {
      object data = "some data";
      var exeResult = new AsyncResult<object>(data);
      var pipeline = new Mock<Pipeline>(MockBehavior.Strict, new object[] { null, null,null });
      pipeline.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(Task.FromResult(exeResult));
      var condition = new Mock<Condition>();
      var context = new ExecutionContext("some value", provider);
      condition.Setup(e => e.Resolve(context)).ReturnsAsync(new AsyncResult<bool>(false));
      var step = new PipelineStep("id", pipeline.Object, condition.Object);
      var er = step.Resolve(context);
      var result = await er.Value();
      Assert.IsNotNull(step.Id);
      Assert.IsNotNull(step.Condition);
      Assert.IsNotNull(step.Pipeline);
      Assert.AreEqual("some data", result);
    }

    [TestMethod]
    public async Task TaskStepWithoutInputTest()
    {
      var methodInfo = typeof(IMockContract).GetMethod("GetData", new List<Type>().ToArray());
      Func<MethodInfo, object, IList<object>, Task> func= (mi, o, lo) => Task.FromResult("some result");
      var method = new ContractMethod(methodInfo, typeof(IMockContract), null, func);   
      var step = new AsyncStep("id", method, null,null );
      var context = new ExecutionContext("some value", provider);
      var er = step.Resolve(context);
      var result = await er.Value();
      Assert.IsNotNull(step.Id);
      Assert.IsNotNull(step.Method);
      Assert.IsNull(step.Condition);
      Assert.IsNull(step.Outputs);
      Assert.AreEqual("some result", result);
    }

    [TestMethod]
    public async Task TaskStepWitInputTest()
    {
      var mockInput = new Mock<Input>(MockBehavior.Strict, new object[] { null });
      mockInput.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(new MockModel { Data = "some input" });
      var mockInputs = new List<Input> { mockInput.Object };
      var methodInfo = typeof(IMockContract).GetMethod("GetData", new List<Type>().ToArray());
      Func<MethodInfo, object, IList<object>, Task> func = (mi, o, lo) => Task.FromResult("some result");
      var method = new ContractMethod(methodInfo, typeof(IMockContract), mockInputs, func);
      var step = new AsyncStep("id", method, null, null);
      var context = new ExecutionContext("some value", provider);
      var er = step.Resolve(context);
      var result = await er.Value();
      Assert.IsNotNull(step.Id);
      Assert.IsNotNull(step.Method);
      Assert.IsNotNull(step.Method.Inputs);
      Assert.AreEqual(1,step.Method.Inputs.Count);
      Assert.IsNull(step.Condition);
      Assert.IsNull(step.Outputs);
      Assert.AreEqual("some result", result);
    }

    [TestMethod]
    public async Task TaskStepWithOutputTest()
    {
      var store = new Dictionary<string, object>();
      var mockOutput = new Mock<Output>(MockBehavior.Strict, new object[] { "var1" });
      mockOutput.Setup(e => e.Resolve(It.IsAny<ExecutionContext>()))
      .Returns(true)  
      .Callback(() =>
      {
        store.Add("var1", "some data");
      });
      var mockOutputs = new List<Output> { mockOutput.Object };

      var methodInfo = typeof(IMockContract).GetMethod("GetData", new List<Type>().ToArray());
      Func<MethodInfo, object, IList<object>, Task> func = (mi, o, lo) => Task.FromResult("some result");
      var method = new ContractMethod(methodInfo, typeof(IMockContract), null, func);
      var step = new AsyncStep("id", method, mockOutputs, null);
      var variables = new List<Variable>();
      var context = new ExecutionContext("some value", provider, variables);
      var er = step.Resolve(context);
      var result = await er.Value();
      Assert.IsNotNull(step.Id);
      Assert.IsNotNull(step.Method);
      Assert.IsNotNull(step.Outputs);
      Assert.IsNull(step.Method.Inputs);
      Assert.IsNull(step.Condition);
      Assert.AreEqual("some result", result);
      Assert.AreEqual(1, store.Count);
      Assert.AreEqual("var1", store.Keys.First());
      Assert.AreEqual("some data", store.Values.First().ToString());

    }

    [TestMethod]
    public async Task TaskStepWithConditionTrueTest()
    {
      var context = new ExecutionContext("some value", provider);
      var condition = new Mock<Condition>();
      condition.Setup(e => e.Resolve(context)).ReturnsAsync(new AsyncResult<bool>(true));

      var methodInfo = typeof(IMockContract).GetMethod("GetData", new List<Type>().ToArray());
      Func<MethodInfo, object, IList<object>, Task> func = (mi, o, lo) => Task.FromResult("some result");
      var method = new ContractMethod(methodInfo, typeof(IMockContract), null, func);

      var step = new AsyncStep("id", method, null, condition.Object);     
      var er = step.Resolve(context);
      var result = await er.Value();
      Assert.IsNotNull(step.Id);
      Assert.IsNotNull(step.Method);
      Assert.IsNotNull(step.Condition);
      Assert.IsNull(step.Method.Inputs);
      Assert.IsNull(step.Outputs);
      Assert.AreEqual("some result", result);
    }
  }
}
