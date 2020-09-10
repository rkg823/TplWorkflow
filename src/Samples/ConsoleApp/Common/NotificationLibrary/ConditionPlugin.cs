using CommonModels;
using NotificationLibrary.Contracts;
using NotificationLibrary.Models;
using System.Threading.Tasks;

namespace NotificationLibrary
{
  public class ConditionPlugin : IConditionPlugin
  {
    public Task<bool> ShouldNotifyExternalSystemWithEmailSchema(Alert alert, NotificationCriteria criteria)
    {
      var evaluation = alert.AlertSource.ToLower() == criteria.Source.ToLower() && alert.AlertCategory.ToLower().StartsWith(criteria.AlertCategoryPrefix.ToLower());
      return Task.FromResult(evaluation);
    }

    public Task<bool> ShouldNotifyExternalSystemWithSmsSchema(Alert alert, NotificationCriteria criteria)
    {
      var evaluation = alert.AlertSource.ToLower() == criteria.Source.ToLower() && alert.AlertCategory.ToLower().StartsWith(criteria.AlertCategoryPrefix.ToLower());
      return Task.FromResult(evaluation);
    }

    public Task<bool> ShouldNotifyExternalSystemWithTwilioSchema(Alert alert, NotificationCriteria criteria)
    {
      var evaluation = alert.AlertSource.ToLower() == criteria.Source.ToLower() && alert.AlertCategory.ToLower().StartsWith(criteria.AlertCategoryPrefix.ToLower());
      return Task.FromResult(evaluation);
    }
  }
}
