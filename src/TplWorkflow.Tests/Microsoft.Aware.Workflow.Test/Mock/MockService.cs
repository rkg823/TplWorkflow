using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TplWorkflow.Test.Mock
{
  public class MockService : IMockContract
  {
    public Task<string> GetData(MockModel input)
    {
      return Task.FromResult(input.Data);
    }

    public Task<string> GetData()
    {
      return Task.FromResult("some result");
    }
  }
}
