using TplWorkflow.Extensions;
using TplWorkflow.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonitoringLibrary.Contracts;
using MonitoringLibrary;
using NotificationLibrary.Contracts;
using NotificationLibrary;
using Microsoft.Extensions.Logging;

namespace MonitoringSample
{
  static class Program
  {
    private static ServiceProvider ServiceProvider;

    static async Task Main(string[] _)
    {
      try
      {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddWorkflow();
        serviceCollection.AddSingleton<TemplateProvider>();

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var wfLoader = ServiceProvider.GetService<IWorkflowLoader>();
        var provider = ServiceProvider.GetService<TemplateProvider>();

        var notification = await provider.LoadNotificationTempalte();

        wfLoader.FromJson(notification.Workflow, notification.Pipelines, notification.Conditions,(ServiceCollection sc)=> {
          sc.AddSingleton<IConditionPlugin, ConditionPlugin>();
          sc.AddSingleton<IMessageCreator, MessageCreator>();
          sc.AddSingleton<IMessagePublisher, MessagePublisher>();
          sc.AddLogging(configure => configure.AddConsole());
        });

        var monitoring = await provider.LoadMonitoringTempalte();

        wfLoader.FromJson(monitoring.Workflow, monitoring.Pipelines, monitoring.Conditions, (ServiceCollection sc) => {
          sc.AddSingleton<IMonitoring, Monitoring>();
          sc.AddSingleton<ICommunication, Communication>();
          sc.AddSingleton<IMapper, Mapper>();
          sc.AddSingleton<IRuleStore, RuleStore>();
          sc.AddLogging(configure => configure.AddConsole());
        });

        await RunMonitoringWorkflow();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    private static async Task RunMonitoringWorkflow()
    {
      var name = "Monitoring";
      var version = 1;
      var wf = ServiceProvider.GetService<IWorkflowHost>();
      var events = CreateEvents();
      _ = await wf.StartAsync(name, version, events);
    }
    
    public static IList<string> CreateEvents()
    {
      return new List<string> {
        "{\"source\":\"RMS\",\"category\":\"Car Speeding\", \"originalIdent\":\"2312\"}",
        "{\"source\":\"RMS\",\"category\":\"Gun Fire\", \"originalIdent\":\"2312\"}"
      };
    }
  }
}
