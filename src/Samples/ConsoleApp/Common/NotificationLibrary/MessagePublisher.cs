using NotificationLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NotificationLibrary
{
  public class MessagePublisher : IMessagePublisher
  {
    public async Task<HttpResponseMessage> PublishAsync(string message, string url)
    {
      await Task.Delay(200);
      var response = new HttpResponseMessage()
      {
        Content = new StringContent($"Successfully published to url: {url}."),
        StatusCode = System.Net.HttpStatusCode.OK,
        RequestMessage = new HttpRequestMessage { }
      };
      Console.WriteLine("");
      Console.WriteLine($"Successfully published to url: {url} and message: {message}");
      Console.WriteLine("");
      return response;
    }
  }
}
