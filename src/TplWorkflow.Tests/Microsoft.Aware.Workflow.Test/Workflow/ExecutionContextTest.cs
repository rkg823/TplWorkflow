using TplWorkflow.Core.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TplWorkflow.Test
{
  [TestClass]
  public class ExecutionContextTest
  {
    [TestMethod]
    public async Task MethodExecutionResultTest()
    {
      var input = "some data";
      var er = new AsyncResult<object>(input);
      var result  = await er.Value();
      Assert.IsNotNull(result);
      Assert.AreEqual(input, result, "Resolve method returns incorrect data.");
    }

    [TestMethod]
    public async Task TaskExecutionResultTest()
    {
      object input = "some data";
      var task = Task.FromResult(input);
      var er = new AsyncResult<object>(task);
      var result = await er.Value();
      Assert.IsNotNull(result);
      Assert.AreEqual(input, result, "Resolve method returns incorrect data.");
    }

  }
}
