// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Outputs
{
  using TplWorkflow.Core.Common;

  public abstract class Output
  {
    public string Name { get; }

    public abstract bool Resolve(ExecutionContext context);

    public Output(string name)
    {
      Name = name;
    }
  }
}
