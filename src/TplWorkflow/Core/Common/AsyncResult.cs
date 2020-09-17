// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using System.Threading.Tasks;

namespace TplWorkflow.Core.Common
{
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
