// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Test.Mock
{
  using System.Threading.Tasks;

  public interface IMockContract
  {
    Task<string> GetData(MockModel input);
    Task<string> GetData();
  }
}
