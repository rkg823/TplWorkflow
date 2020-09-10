using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CommonModels
{
  public class Template
  {
    public string Workflow { get; set; }
    public IList<string> Pipelines { get; set; }
    public IList<string> Conditions { get; set; }
    public Action<ServiceCollection> Dependency { get; set; }

  }
}
