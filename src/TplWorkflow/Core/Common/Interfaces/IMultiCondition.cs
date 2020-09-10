using TplWorkflow.Core.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplWorkflow.Core.Common.Interfaces
{
  public interface IMultiCondition
  {
    IList<Condition> Conditions { get; }
  }
}
