using CommonModels;
using Microsoft.Extensions.Logging;
using NotificationLibrary.Contracts;
using NotificationLibrary.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationLibrary
{
  public class ConditionPlugin : IConditionPlugin
  {
    private readonly ILogger<ConditionPlugin> logger;

    public ConditionPlugin(ILogger<ConditionPlugin> logger)
    {
      this.logger = logger;
    }
    public Task<bool> ShouldNotifyExternalSystemWithEmailSchema(Alert alert, NotificationCriteria criteria)
    {
      var evaluation = alert.AlertSource.ToLower() == criteria.Source.ToLower() && alert.AlertCategory.ToLower().StartsWith(criteria.AlertCategoryPrefix.ToLower());
      logger.LogInformation($"Condition ShouldNotifyExternalSystemWithSmsSchema - evaluation {evaluation} - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return Task.FromResult(evaluation);
    }

    public Task<bool> ShouldNotifyExternalSystemWithSmsSchema(Alert alert, NotificationCriteria criteria)
    {
      var evaluation = alert.AlertSource.ToLower() == criteria.Source.ToLower() && alert.AlertCategory.ToLower().StartsWith(criteria.AlertCategoryPrefix.ToLower());
      logger.LogInformation($"Condition ShouldNotifyExternalSystemWithSmsSchema - evaluation {evaluation} - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return Task.FromResult(evaluation);
    }

    public Task<bool> ShouldNotifyExternalSystemWithTwilioSchema(Alert alert, NotificationCriteria criteria)
    {    
      var evaluation = alert.AlertSource.ToLower() == criteria.Source.ToLower() && alert.AlertCategory.ToLower().StartsWith(criteria.AlertCategoryPrefix.ToLower());
      logger.LogInformation($"Condition ShouldNotifyExternalSystemWithTwilioSchema - evaluation {evaluation} - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return Task.FromResult(evaluation);
    }
  }
}
