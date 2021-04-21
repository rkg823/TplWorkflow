using CommonModels;
using Microsoft.Extensions.Logging;
using MonitoringLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringLibrary
{
  public class Monitoring : IMonitoring
  {
    private readonly ILogger<Monitoring> logger;
    public Monitoring(ILogger<Monitoring> logger)
    {
      this.logger = logger;
    }
    public Task<IList<CommonModels.Action>> Evaluate(Activity activity, IList<Rule> rules)
    {
      logger.LogInformation($"Evaluate - Thread: {Thread.CurrentThread.ManagedThreadId}");
      IList<CommonModels.Action> actions = new List<CommonModels.Action> {
          //new CommonModels.Action { Id = Guid.NewGuid().ToString().ToString(), Data = "this is an action1", Activity = activity },
          //new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action2", Activity = activity },
          //new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action3", Activity = activity },
          //new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action4", Activity = activity },
          //new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action5", Activity = activity },
          //new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action6", Activity = activity },
          //new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action7", Activity = activity },
          //new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action8", Activity = activity },
          //new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action9", Activity = activity },
          //new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action10", Activity = activity },
          new CommonModels.Action { Id = Guid.NewGuid().ToString(), Data = "this is an action11", Activity = activity }
      };
      return Task.FromResult(actions);
    }


  }
}
