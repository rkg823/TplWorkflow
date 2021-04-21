using CommonModels;
using Microsoft.Extensions.Logging;
using NotificationLibrary.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationLibrary
{
  public class MessageCreator : IMessageCreator
  {
    private readonly ILogger<MessageCreator> logger;

    public MessageCreator(ILogger<MessageCreator> logger)
    {
      this.logger = logger;
    }

    public async Task<string> CreateNotificationMessageAsync(Alert alert, string templateName)
    {
      await Task.Delay(10);
      logger.LogInformation($"Created Notification Message - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return $"notification message for alertid: {alert.AlertId} | alert category: {alert.AlertCategory} || template: {templateName}";
    }

  }
}
