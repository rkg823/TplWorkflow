// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using System.Threading.Tasks;

namespace TplWorkflow.Test.Mock
{
  public interface IMockContract
  {
    Task<string> GetData(MockModel input);
    Task<string> GetData();
  }
}
