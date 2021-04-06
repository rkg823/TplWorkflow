// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Extensions.Mappers
{
  using TplWorkflow.Core.Maps;
  using TplWorkflow.Models.Templates;
  using System.Collections.Generic;

  public static class MapMapperExtension
  {
    public static IList<Map> Map(this IList<MapTemplate> templates)
    {
      var list = new List<Map>();

      foreach (var template in templates)
      {
        list.Add(template.Map());
      }

      return list;
    }

    public static Map Map(this MapTemplate template)
    {
      template.From.DataType = typeof(object).AssemblyQualifiedName;

      var from = template.From.Map();

      return new Map(from, template.To);
    }

  }
}
