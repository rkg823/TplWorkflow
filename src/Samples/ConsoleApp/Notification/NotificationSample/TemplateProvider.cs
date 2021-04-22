using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TplWorkflow.Models.Templates;
using System.Linq;

namespace NotificationSample
{
  public class TemplateProvider
  {
    const string NOTIFICAITON_TEMPLATE_PATH = "Templates\\Notification";
    const string ConditionDirectory = "Conditions";
    const string PipelineDirectory = "Pipelines";
    const string WorkflowFile = "Workflow.json";


    public async Task<(WorkflowTemplate template, TemplateContext context)> LoadWfTemplate()
    {
      var workflow = await LoadFile($"{NOTIFICAITON_TEMPLATE_PATH}\\{WorkflowFile}");
      var conditions = await LoadFiles($"{NOTIFICAITON_TEMPLATE_PATH}\\{ConditionDirectory}");
      var pipelines = await LoadFiles($"{NOTIFICAITON_TEMPLATE_PATH}\\{PipelineDirectory}");

      var wfTemplate = JsonConvert.DeserializeObject<WorkflowTemplate>(workflow);

      var conditionTemplates = conditions.Select(c => JsonConvert.DeserializeObject<ConditionTemplate>(c)).ToList();
      var pipelineTemplates = pipelines.Select(p => JsonConvert.DeserializeObject<PipelineTemplate>(p)).ToList();
      var context = new TemplateContext(pipelineTemplates, conditionTemplates);

      return (wfTemplate, context);
    }

    public static async Task<string> LoadFile(string path)
    {
      using StreamReader sr = new StreamReader(path);
      return await sr.ReadToEndAsync();
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
