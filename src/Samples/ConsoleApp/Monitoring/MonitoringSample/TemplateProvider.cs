using CommonModels;
using Microsoft.Extensions.DependencyInjection;
using MonitoringLibrary;
using MonitoringLibrary.Contracts;
using NotificationLibrary;
using NotificationLibrary.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MonitoringSample
{
  public class TemplateProvider
  {
    const string NotificationTemplatePath = "Templates\\Notification";
    const string MonitoringTemplatePath = "Templates\\Monitoring";
    const string ConditionDirectory = "Conditions";
    const string PipelineDirectory = "Pipelines";
    const string WorkflowFile = "Workflow.json";


    public async Task<Template> LoadMonitoringTempalte()
    {
      return await LoadTemplate(MonitoringTemplatePath);
    }

    public async Task<Template> LoadNotificationTempalte()
    {
      return await LoadTemplate(NotificationTemplatePath);
    }
    public async Task<Template> LoadTemplate(string path)
    {
      var tempalte = new Template
      {
        Workflow = await LoadFile($"{path}\\{WorkflowFile}"),
        Conditions = await LoadFiles($"{path}\\{ConditionDirectory}"),
        Pipelines = await LoadFiles($"{path}\\{PipelineDirectory}")
      };
      return tempalte;
    }

    public static async Task<string> LoadFile(string path)
    {
      using (StreamReader sr = new StreamReader(path))
      {
        return await sr.ReadToEndAsync();
      }
    }
    private async Task<IList<string>> LoadFiles(string dir)
    {
      var list = new List<string>();
      if (!Directory.Exists(dir)) return list;
      string[] filePaths = Directory.GetFiles(dir);
      foreach (var path in filePaths)
      {
        list.Add(await LoadFile(path));
      }
      return list;
    }



  }
}
