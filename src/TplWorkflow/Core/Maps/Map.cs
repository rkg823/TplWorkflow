// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Core.Common;
using TplWorkflow.Core.Inputs;

namespace TplWorkflow.Core.Maps
{
  public class Map
  {
    public Input From { get; }
    public string To { get; }
    public Map(Input from, string to) 
    {
      From = from;
      To = to;
    }
    public Variable Resolve(ExecutionContext context) 
    {
      var from = From.Resolve(context);
      return new Variable(To, from);    
    }
  }
}
