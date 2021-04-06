// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Core.Methods
{
  using TplWorkflow.Core.Common;
  using TplWorkflow.Core.Inputs;
  using System.Collections.Generic;
  using System.Reflection;
  using System.Threading.Tasks;

  public abstract class Method
  {
    public MethodInfo MethodInfo { get; }
    public IList<Input> Inputs { get; }
    public abstract Task Resolve(ExecutionContext context);

    protected Method(MethodInfo info, IList<Input> inputs)
    {
      MethodInfo = info;
      Inputs = inputs;
    }
  }
}
