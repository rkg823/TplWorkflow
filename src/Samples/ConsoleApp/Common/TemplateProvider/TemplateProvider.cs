using CommonModels;
using Microsoft.Extensions.DependencyInjection;
using MonitoringLibrary;
using MonitoringLibrary.Contracts;
using NotificationLibrary;
using NotificationLibrary.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TemplateProvider
{
  public class TemplateProvider: ITemplateProvider
  {
    private static readonly string NotificationTemplatePath = "Templates\\Notification";
    private static readonly string MonitoringTemplatePath = "Templates\\Monitoring";
    private static readonly string ConditionDirectory = "Conditions";
    private static readonly string PipelineDirectory = "Pipelines";
    private static readonly string WorkflowFile = "Workflow.json";


    public async Task<Template> LoadMonitoringTempalte()
    {
      var template = await LoadTemplate(MonitoringTemplatePath);
      template.Dependency = (sp) =>
      {
        sp.AddSingleton<IMonitoring, Monitoring>();
        sp.AddSingleton<ICommunication, Communication>();
        sp.AddSingleton<IMapper, Mapper>();
        sp.AddSingleton<IRuleStore, RuleStore>();
        sp.AddSingleton<ILogger, Logger>();
      };
      return template;
    }

    public async Task<Template> LoadNotificationTempalte()
    {
      var template = await LoadTemplate(NotificationTemplatePath);
      template.Dependency = (sp) =>
      {
        sp.AddSingleton<IConditionPlugin, ConditionPlugin>();
        sp.AddSingleton<IMessageCreator, MessageCreator>();
        sp.AddSingleton<IMessagePublisher, MessagePublisher>();
      };
      return template;
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
