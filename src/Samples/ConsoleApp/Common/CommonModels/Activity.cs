namespace CommonModels
{
  public class Activity
  {
    public string ActivityId { get; set; }
    public string ActivitySource { get; set; }
    public string ActivityCategory { get; set; }
    public string OriginalIdent { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }
}
