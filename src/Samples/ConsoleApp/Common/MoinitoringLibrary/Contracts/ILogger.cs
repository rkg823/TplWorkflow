using System.Threading.Tasks;

namespace MonitoringLibrary.Contracts
{
  public interface ILogger
  {
    void Error(string message);
    void Info(string message);
    void Warning(string message);
  }
}