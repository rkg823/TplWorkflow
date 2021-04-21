using CommonModels;
using Microsoft.Extensions.Logging;
using MonitoringLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringLibrary
{
  public class RuleStore : IRuleStore
  {
    private readonly ILogger<RuleStore> logger;
    public RuleStore(ILogger<RuleStore> logger)
    {
      this.logger = logger;
    }

    public Task<IList<Rule>> GetRules(Activity activity)
    {
      logger.LogInformation($"GetRules- Thread: {Thread.CurrentThread.ManagedThreadId}");
      IList<Rule> rules = new List<Rule> { new Rule { Id = Guid.NewGuid().ToString(), Data = "this is a rule" } };
      return Task.FromResult(rules);
    }

    public Task<Tuple<Activity, IList<Rule>>> GetRulesWithActivity(Activity activity)
    {
      logger.LogInformation($"GetRules- Thread: {Thread.CurrentThread.ManagedThreadId}");
      IList<Rule> rules = new List<Rule> { new Rule { Id = Guid.NewGuid().ToString(), Data = "this is a rule" } };
      return Task.FromResult(Tuple.Create(activity,rules));
    }
  }
}
