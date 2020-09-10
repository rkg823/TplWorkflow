using System;

namespace CommonModels
{
  public class Alert
  {
    public string AlertTitle { get; set; }
    public string AlertDescription { get; set; }
    public DateTime? AlertDate { get; set; }
    public int AlertSeverity { get; set; }
    public string AlertReason { get; set; }
    public bool IsPinned { get; set; }
    public bool IsMarkedForUpdate { get; set; }
    public long? MaxAlertId { get; set; }
    public bool IsForMaxAlertOnly { get; set; }
    public string AlertCategory { get; set; }
    public string Reason { get; set; }
    public int? RulePriority { get; set; }
    public string PriorityCode { get; set; }
    public bool? IsGangRelated { get; set; }
    public string Status { get; set; }
    public string UserCadId { get; set; }
    public DateTime? ActivityDate { get; set; }
    public string CorrelationReason { get; set; }
    public string TimeAndDistanceDifference { get; set; }
    public string OriginalEvent { get; set; }
    public string Notes { get; set; }
    public string AlertSource { get; set; }
    public string AlertId { get; set; }
    public string ActivityId { get; set; }

  }
}
