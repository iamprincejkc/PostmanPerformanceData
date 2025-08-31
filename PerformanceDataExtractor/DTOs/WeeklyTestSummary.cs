namespace PerformanceDataExtractor.DTOs;

public class WeeklyTestSummary
{
    public string TestName { get; set; } = string.Empty;
    public DateTime TestDate { get; set; }
    public int TotalRequests { get; set; }
    public double Throughput { get; set; }
    public double AverageResponseTime { get; set; }
    public double ErrorRate { get; set; }
    public int RequestTypesCount { get; set; }
}
