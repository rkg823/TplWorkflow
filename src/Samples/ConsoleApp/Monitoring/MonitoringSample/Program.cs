using TplWorkflow.Extensions;
using TplWorkflow.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateProvider;

namespace MonitoringSample
{
  static class Program
  {
    private static ServiceProvider ServiceProvider;
    static async Task Main(string[] args)
    {
      try
      {
        ConfigureDependency();
        var wfLoader = ServiceProvider.GetService<IWorklowLoader>();
        var provider = ServiceProvider.GetService<ITemplateProvider>();
        var notification = await provider.LoadNotificationTempalte();
        wfLoader.FromJson(notification.Workflow, notification.Pipelines, notification.Conditions, notification.Dependency);
        var monitoring = await provider.LoadMonitoringTempalte();
        wfLoader.FromJson(monitoring.Workflow, monitoring.Pipelines, monitoring.Conditions, monitoring.Dependency);
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
      var result = await wf.StartAsync(name, version, events);
    }
    
    public static IList<string> CreateEvents()
    {
      return new List<string> {
        "{\"source\":\"RMS\",\"category\":\"Car Speeding\", \"originalIdent\":\"2312\"}",
        "{\"source\":\"RMS\",\"category\":\"Gun Fire\", \"originalIdent\":\"2312\"}"
      };
    }

    private static void ConfigureDependency()
    {
      var serviceCollection = new ServiceCollection();
      ConfigureServices(serviceCollection);
      ServiceProvider = serviceCollection.BuildServiceProvider();
    }
    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
      serviceCollection.AddWorkflow();
      serviceCollection.AddSingleton<ITemplateProvider, TemplateProvider.TemplateProvider>();
    }

  }
}
