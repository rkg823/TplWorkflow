using CommonModels;
using NotificationLibrary.Contracts;
using System.Threading.Tasks;

namespace NotificationLibrary
{
  public class MessageCreator : IMessageCreator
  {
    public async Task<string> CreateNotificationMessageAsync(Alert alert, string templateName)
    {
      await Task.Delay(10);
      return $"notification message for alertid: {alert.AlertId} | alert category: {alert.AlertCategory} || template: {templateName}";
    }

  }
}
