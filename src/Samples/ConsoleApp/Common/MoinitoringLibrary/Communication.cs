using Microsoft.Extensions.Logging;
using MonitoringLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringLibrary
{
  public class Communication : ICommunication
  {
    private readonly ILogger<Communication> logger;
    public Communication(ILogger<Communication> logger)
    {
      this.logger = logger;
    }
    public async Task<T> SendToIotHub<T>(T data)
    {     
      await Task.Delay(1000);
      logger.LogInformation($"Send to iot hub - type:{typeof(T)} - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return data;
    }

    public async Task<T> SendToIotHub<T>(T data, string x)
    {
      await Task.Delay(1000);
      logger.LogInformation($"Send to iot hub - type:{typeof(T)} - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return data;
    }


    public async Task<T> SendToNotificationHub<T>(T data)
    {
      await Task.Delay(100);
      logger.LogInformation($"Send to notification hub - - type:{typeof(T)} - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return data;
    }


    public async Task<IList<T>> SendToIotHub<T>(IList<T> data)
    {
      await Task.Delay(1000);
      logger.LogInformation($"Send to iot hub - type:{typeof(T)} - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return data;
    }
    public async Task<IList<T>> SendToNotificationHub<T>(IList<T> data)
    {
      await Task.Delay(100);
      logger.LogInformation($"Send to notification hub - - type:{typeof(T)} - Thread: {Thread.CurrentThread.ManagedThreadId}");
      return data;
    }
  }
}
