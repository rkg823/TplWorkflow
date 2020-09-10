using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TplWorkflow.Test.Mock
{
  public interface IMockContract
  {
    Task<string> GetData(MockModel input);
    Task<string> GetData();
  }
}
