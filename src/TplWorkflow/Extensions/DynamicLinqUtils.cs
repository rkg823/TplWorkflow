// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Extensions
{
  using System.Collections.Generic;
  using System.Linq.Dynamic.Core.CustomTypeProviders;

  [DynamicLinqType]
  public static class Utils
  {
    public static bool HasKey(object valuePairs, string code)
    {
      var dictionary = (Dictionary<string, object>)valuePairs;
      return dictionary.ContainsKey(code);
    }
  }
}
