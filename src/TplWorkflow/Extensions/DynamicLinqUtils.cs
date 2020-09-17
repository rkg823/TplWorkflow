// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace TplWorkflow.Extensions
{
  [DynamicLinqType]
  public static class Utils
  {
    public static bool HasKey(object valuePairs, string code)
    {
      var d = (Dictionary<string, object>)valuePairs;
      return d.ContainsKey(code);
    }
  }
}
