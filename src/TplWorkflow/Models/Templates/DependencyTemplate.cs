using System;
using System.Collections.Generic;
using System.Text;

namespace TplWorkflow.Models.Templates
{
  public class DependencyTemplate
  {
    public string Contract { get; set; }
    public string Implementation { get; set; }
    public string Lifetime { get; set; }
  }
}
