using CommonModels;
using NotificationLibrary.Models;
using System.Threading.Tasks;

namespace NotificationLibrary.Contracts
{
  public interface IConditionPlugin
  {
    Task<bool> ShouldNotifyExternalSystemWithEmailSchema(Alert alert, NotificationCriteria criteria);
    Task<bool> ShouldNotifyExternalSystemWithSmsSchema(Alert alert, NotificationCriteria criteria);
    Task<bool> ShouldNotifyExternalSystemWithTwilioSchema(Alert alert, NotificationCriteria criteria);
  }
}