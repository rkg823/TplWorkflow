using CommonModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringLibrary.Contracts
{
  public interface IRuleStore
  {
    Task<IList<Rule>> GetRules(Activity activity);
    Task<Tuple<Activity, IList<Rule>>> GetRulesWithActivity(Activity activity);
  }
}