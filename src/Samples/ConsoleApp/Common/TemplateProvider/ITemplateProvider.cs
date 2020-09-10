using CommonModels;
using System.Threading.Tasks;

namespace TemplateProvider
{
  public interface ITemplateProvider
  {
    Task<Template> LoadMonitoringTempalte();
    Task<Template> LoadNotificationTempalte();
    Task<Template> LoadTemplate(string path);
  }
}