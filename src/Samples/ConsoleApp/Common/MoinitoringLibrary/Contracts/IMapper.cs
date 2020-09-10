using CommonModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringLibrary.Contracts
{
  public interface IMapper
  {
    Task<Alert> ActionToAlert(Action action);
    Task<Activity> EventToActivity(string _event);
    Task<IList<Alert>> ActionToAlert(IList<CommonModels.Action> actions);
  }
}