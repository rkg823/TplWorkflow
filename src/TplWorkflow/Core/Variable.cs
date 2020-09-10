namespace TplWorkflow.Core
{
  public class Variable
  {
    public string Name { get; }
    public object Data { get; }

    public Variable(string name, object data)
    {
      Name = name;
      Data = data;
    }
  }
}
