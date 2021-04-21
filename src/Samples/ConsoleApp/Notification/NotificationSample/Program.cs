using CommonModels;
using TplWorkflow.Extensions;
using TplWorkflow.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateProvider;
using TplWorkflow.Services;
using NotificationLibrary.Contracts;
using NotificationLibrary;
using Microsoft.Extensions.Logging;

namespace NotificationSample
{
  class Program
  {
    private static ServiceProvider ServiceProvider;
    static async Task Main(string[] args)
    {
      try
      {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddWorkflow();
        serviceCollection.AddSingleton<IConditionPlugin, ConditionPlugin>();
        serviceCollection.AddSingleton<IMessageCreator, MessageCreator>();
        serviceCollection.AddSingleton<IMessagePublisher, MessagePublisher>();
        serviceCollection.AddSingleton<ITemplateProvider, TemplateProvider.TemplateProvider>();
        serviceCollection.AddLogging(configure => configure.AddConsole());

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var wfLoader = ServiceProvider.GetService<IWorkflowLoader>();
        var provider = ServiceProvider.GetService<ITemplateProvider>();

        var template = await provider.LoadNotificationTempalte();

        wfLoader.FromJson(template.Workflow, template.Pipelines, template.Conditions);

        await RunNotificaitonWorkflow();
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    private static async Task RunNotificaitonWorkflow()
    {
      var name = "ChannelNotification";
      var version = 1;
      var wf = ServiceProvider.GetService<IWorkflowHost>();
      var alerts = CreateAlerts();
      var result = await wf.StartAsync(name, version, alerts);
    }
    private static IList<Alert> CreateAlerts()
    {
      return new List<Alert> {
          new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "RMS",
          AlertCategory = "Test"
        },
           new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "RMS",
          AlertCategory = "Car Speeding"
        },
          new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "RMS",
          AlertCategory = "Gun Fire"
        },
            new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "RMS",
          AlertCategory = "Test"
        },
            new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "RMS",
          AlertCategory = "Gun Fire"
        },
         new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "RMS",
          AlertCategory = "Watchlist Vehicle"
        },
         new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "Test",
          AlertCategory = "Some Random",
          AlertSeverity = 2,
          Notes = "Some Alert note"
        },
                   new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "RMS",
          AlertCategory = "Watchlist Vehicle"
        },
                    new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "RMS",
          AlertCategory = "Test"
        },
                    new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "RMS",
          AlertCategory = "Test"
        },
                      new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "Test",
          AlertCategory = "Some Random",
          AlertSeverity = 1,
          Notes = "Some Alert note"
        },
                      new Alert {
          AlertId = Guid.NewGuid().ToString(),
          AlertSource = "Test",
          AlertCategory = "Some Random",
          AlertSeverity = 2,
          Notes = "Some Alert note"
        },
      };
    }
  }
}
