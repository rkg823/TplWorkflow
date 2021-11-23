// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core
{
  public class Variable
  {
    public string Name { get; }
    public object Data { get; }

    public Varia  ble(string name, object data)
    {
      Name = name;
      Data = data;
    }
  }
}
