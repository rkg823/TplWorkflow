using CommonModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringLibrary.Contracts
{
  public interface IMonitoring
  {
    Task<IList<Action>> Evaluate(Activity activity, IList<Rule> rules);
  }
}