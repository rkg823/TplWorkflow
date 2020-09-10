using MonitoringLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringLibrary
{
  public class Logger : ILogger
  {
    public void Error(string message)
    {
      Console.WriteLine($"Error - {message}");
    }

    public void Info(string message)
    {
      Console.WriteLine($"Info - {message}");
    }

    public void Warning(string message)
    {
      Console.WriteLine($"Warning - {message}");
    }
  }
}
