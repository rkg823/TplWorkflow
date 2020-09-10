using TplWorkflow.Models;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Pipelines;
using TplWorkflow.Core.Steps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TplWorkflow.Stores;
using TplWorkflow.Core;
using TplWorkflow.Core.Maps;
using TplWorkflow.Core.Common;

namespace TplWorkflow.Test
{
  [TestClass]
  public class PipelineTest
  {
    private ServiceProvider provider = null;
    [TestInitialize]
    public void Init()
    {
      provider = new ServiceCollection().BuildServiceProvider();
    }

    [TestMethod]
    public async Task ParallelPipelineTest()
    {
      var i1 = "i1";
      object r1 = "r1";
      string r2 = "r2";
      var context = new ExecutionContext(i1, provider);
      var step1 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult1 = new AsyncResult<object>(Task.FromResult(r1));
      step1.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult1);
      var step2 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult2 = new AsyncResult<object>(r2);
      step2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult2);
      var steps = new List<Step> { step1.Object, step2.Object };
      var variables = new List<Variable>();
      var maps = new List<Map>();
      var pipeline = new ParallelPipeline(null, steps, variables, maps);
      var task = await pipeline.Resolve(context);
      var result = (IList<object>)await task.Value();
      Assert.IsNull(pipeline.Condition);
      Assert.IsNotNull(pipeline.Steps);
      Assert.AreEqual(2, pipeline.Steps.Count);
      Assert.IsNotNull(result);
      Assert.AreEqual(2, result.Count);
      Assert.AreEqual("r1", result[0] as string);
      Assert.AreEqual("r2", result[1] as string);
    }

    [TestMethod]
    public async Task ParallelPipelineWithTrueConditionTest()
    {
      var i1 = "i1";
      object r1 = "r1";
      string r2 = "r2";
      var context = new ExecutionContext(i1, provider);
      var step1 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult1 = new AsyncResult<object>(Task.FromResult(r1));
      step1.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult1);
      var step2 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult2 = new AsyncResult<object>(r2);
      step2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult2);
      var steps = new List<Step> { step1.Object, step2.Object };
      var condition = new Mock<Condition>();
      condition.Setup(e => e.Resolve(context)).ReturnsAsync(new AsyncResult<bool>(true));
      var variables = new List<Variable>();
      var maps = new List<Map>();
      var pipeline = new ParallelPipeline(condition.Object, steps, variables, maps);
      var task = await pipeline.Resolve(context);
      var result = (IList<object>)await task.Value();
      Assert.IsNotNull(pipeline.Condition);
      Assert.IsNotNull(pipeline.Steps);
      Assert.AreEqual(2, pipeline.Steps.Count);
      Assert.IsNotNull(result);
      Assert.AreEqual(2, result.Count);
      Assert.AreEqual("r1", result[0] as string);
      Assert.AreEqual("r2", result[1] as string);
    }

    [TestMethod]
    public async Task ParallelPipelineWithFalseConditionTest()
    {
      var i1 = "i1";
      object r1 = "r1";
      string r2 = "r2";
      var context = new ExecutionContext(i1, provider);
      var step1 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult1 = new AsyncResult<object>(Task.FromResult(r1));
      step1.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult1);
      var step2 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult2 = new AsyncResult<object>(r2);
      step2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult2);
      var steps = new List<Step> { step1.Object, step2.Object };
      var condition = new Mock<Condition>();
      condition.Setup(e => e.Resolve(context)).ReturnsAsync(new AsyncResult<bool>(false));
      var variables = new List<Variable>();
      var maps = new List<Map>();
      var pipeline = new ParallelPipeline(condition.Object, steps, variables,maps);
      var task = await pipeline.Resolve(context);
      var result = (IList<object>)await task.Value();
      Assert.IsNotNull(pipeline.Condition);
      Assert.IsNotNull(pipeline.Steps);
      Assert.AreEqual(2, pipeline.Steps.Count);
      Assert.IsNotNull(result);
      Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task SequentialPipelineTest()
    {
      var i1 = "i1";
      object r1 = "r1";
      string r2 = "r2";
      var context = new ExecutionContext(i1, provider);
      var step1 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult1 = new AsyncResult<object>(Task.FromResult(r1));
      step1.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult1);
      var step2 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult2 = new AsyncResult<object>(r2);
      step2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult2);
      var steps = new List<Step> { step1.Object, step2.Object };
      var variables = new List<Variable>();
      var maps = new List<Map>();
      var pipeline = new SequentialPipeline(null, steps, variables, maps);
      var task = await pipeline.Resolve(context);
      var result = (string)await task.Value();
      Assert.IsNull(pipeline.Condition);
      Assert.IsNotNull(pipeline.Steps);
      Assert.AreEqual(2, pipeline.Steps.Count);
      Assert.IsNotNull(result);
      Assert.AreEqual("r2", result);
    }

    [TestMethod]
    public async Task SequentialPipelineWithTrueConditionTest()
    {
      var i1 = "i1";
      object r1 = "r1";
      string r2 = "r2";
      var context = new ExecutionContext(i1, provider);
      var step1 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult1 = new AsyncResult<object>(Task.FromResult(r1));
      step1.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult1);
      var step2 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult2 = new AsyncResult<object>(r2);
      step2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult2);
      var steps = new List<Step> { step1.Object, step2.Object };
      var condition = new Mock<Condition>();
      condition.Setup(e => e.Resolve(context)).ReturnsAsync(new AsyncResult<bool>(true));
      var variables = new List<Variable>();
      var maps = new List<Map>();
      var pipeline = new SequentialPipeline(condition.Object, steps, variables, maps);
      var task = await pipeline.Resolve(context);
      var result = (string)await task.Value();
      Assert.IsNotNull(pipeline.Condition);
      Assert.IsNotNull(pipeline.Steps);
      Assert.AreEqual(2, pipeline.Steps.Count);
      Assert.IsNotNull(result);
      Assert.AreEqual("r2", result);
    }

    [TestMethod]
    public async Task SequentialPipelineWithFalseConditionTest()
    {
      var i1 = "i1";
      object r1 = "r1";
      string r2 = "r2";
      var context = new ExecutionContext(i1, provider);
      var step1 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult1 = new AsyncResult<object>(Task.FromResult(r1));
      step1.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult1);
      var step2 = new Mock<Step>(MockBehavior.Strict, new object[] { "id", null });
      var taskResult2 = new AsyncResult<object>(r2);
      step2.Setup(e => e.Resolve(It.IsAny<ExecutionContext>())).Returns(taskResult2);
      var steps = new List<Step> { step1.Object, step2.Object };
      var condition = new Mock<Condition>();
      condition.Setup(e => e.Resolve(context)).ReturnsAsync(new AsyncResult<bool>(false));
      var variables = new List<Variable>();
      var maps = new List<Map>();
      var pipeline = new SequentialPipeline(condition.Object, steps, variables, maps);
      var task = await pipeline.Resolve(context);
      var result =  task.Value();
      Assert.IsNotNull(pipeline.Condition);
      Assert.IsNotNull(pipeline.Steps);
      Assert.AreEqual(2, pipeline.Steps.Count);
      Assert.IsNull(result);
    }
  }
}
