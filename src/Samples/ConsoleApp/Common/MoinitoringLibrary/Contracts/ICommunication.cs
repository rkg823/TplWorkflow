using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringLibrary.Contracts
{
  public interface ICommunication
  {
    Task<T> SendToIotHub<T>(T data);
    Task<T> SendToIotHub<T>(T data, string x);
    Task<T> SendToNotificationHub<T>(T data);
    Task<IList<T>> SendToIotHub<T>(IList<T> data);
    Task<IList<T>> SendToNotificationHub<T>(IList<T> data);
  }
}