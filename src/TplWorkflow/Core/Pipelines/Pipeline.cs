using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Common;
using TplWorkflow.Core.Common.Interfaces;
using TplWorkflow.Core.Maps;
using TplWorkflow.Core.Steps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TplWorkflow.Core.Pipelines
{
  public abstract class Pipeline: IConditional
  {
    public Condition Condition { get; }
    public IList<Variable> Variables { get; }
    public IList<Map> Maps { get; }

    protected Pipeline(Condition condition, IList<Variable> variables, IList<Map> maps)
    {
      Condition = condition;
      Variables = variables;
      Maps = maps;
    }

    public abstract Task<AsyncResult<object>> Resolve(ExecutionContext context);
  }
}
