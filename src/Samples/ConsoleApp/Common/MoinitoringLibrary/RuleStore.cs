using CommonModels;
using MonitoringLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringLibrary
{
  public class RuleStore : IRuleStore
  {
    private readonly ILogger logger;
    public RuleStore(ILogger logger)
    {
      this.logger = logger;
    }

    public Task<IList<Rule>> GetRules(Activity activity)
    {
      Console.WriteLine($"GetRules- Thread: {Thread.CurrentThread.ManagedThreadId}");
      IList<Rule> rules = new List<Rule> { new Rule { Id = Guid.NewGuid().ToString(), Data = "this is a rule" } };
      return Task.FromResult(rules);
    }

    public Task<Tuple<Activity, IList<Rule>>> GetRulesWithActivity(Activity activity)
    {
      Console.WriteLine($"GetRules- Thread: {Thread.CurrentThread.ManagedThreadId}");
      IList<Rule> rules = new List<Rule> { new Rule { Id = Guid.NewGuid().ToString(), Data = "this is a rule" } };
      return Task.FromResult(Tuple.Create(activity,rules));
    }
  }
}
