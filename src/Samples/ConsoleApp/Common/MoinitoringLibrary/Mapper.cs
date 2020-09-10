using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MonitoringLibrary.Contracts;
using CommonModels;

namespace MonitoringLibrary
{
  public class Mapper : IMapper
  {
    private readonly ILogger logger;
    public Mapper(ILogger logger)
    {
      this.logger = logger;
    }
    public Task<Activity> EventToActivity(string _event)
    {
      var jsonObject = JsonConvert.DeserializeObject<dynamic>(_event);
      var activity = new Activity
      {
        ActivityId = Guid.NewGuid().ToString(),
        ActivitySource = jsonObject.source,
        ActivityCategory = jsonObject.category,
        OriginalIdent = jsonObject.originalIdent
      };
     logger.Info($"EventToActivity - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return Task.FromResult(activity);
    }

    public Task<Alert> ActionToAlert(CommonModels.Action action)
    {
     logger.Info($"ActionToAlert - Thread: {Thread.CurrentThread.ManagedThreadId}");
      var alert = new Alert { 
        AlertId = Guid.NewGuid().ToString(), 
        AlertCategory = action.Activity.ActivityCategory,
        AlertSource = action.Activity.ActivitySource
      };
      return Task.FromResult(alert);
    }

    public async Task<IList<Alert>> ActionToAlert(IList<CommonModels.Action> actions)
    {
      logger.Info($"ActionToAlert - Thread: {Thread.CurrentThread.ManagedThreadId}");
      IList<Alert> alerts = new List<Alert>();
      foreach(var action in actions)
      {
        alerts.Add(await ActionToAlert(action));
      }
      return alerts;
    }
  }
}
