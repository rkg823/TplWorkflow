using Microsoft.Extensions.Logging;
using NotificationLibrary.Contracts;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationLibrary
{
  public class MessagePublisher : IMessagePublisher
  {
    private readonly ILogger<MessagePublisher> logger;

    public MessagePublisher(ILogger<MessagePublisher> logger)
    {
      this.logger = logger;
    }
    public async Task<HttpResponseMessage> PublishAsync(string message, string url)
    {  
      await Task.Delay(200);
      var response = new HttpResponseMessage()
      {
        Content = new StringContent($"Successfully published to url: {url}."),
        StatusCode = System.Net.HttpStatusCode.OK,
        RequestMessage = new HttpRequestMessage { }
      };

      logger.LogInformation($"Publish Message - Thread: {Thread.CurrentThread.ManagedThreadId}");
      logger.LogInformation($"Successfully published to url: {url} and message: {message}");
      return response;
    }
  }
}
