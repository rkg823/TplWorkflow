using CommonModels;
using NotificationLibrary.Models;
using System.Threading.Tasks;

namespace NotificationLibrary.Contracts
{
  public interface IMessageCreator
  {
    Task<string> CreateNotificationMessageAsync(Alert alert, string templateName);
  }
}