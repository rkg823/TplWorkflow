// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Common
{
  using System.Threading.Tasks;

  public class AsyncResult<T>
  {
    private Task<T> Data { get; }
    public AsyncResult(Task<T> data)
    {
      Data = data;
    }
    public AsyncResult(T data)
    {
      Data = Task.FromResult(data);
    }

    public Task<T> Value()
    {
      return Data;
    }
  }
}
