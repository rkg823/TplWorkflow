using System.Net.Http;
using System.Threading.Tasks;

namespace NotificationLibrary.Contracts
{
  public interface IMessagePublisher
  {
    Task<HttpResponseMessage> PublishAsync(string message, string url);
  }
}