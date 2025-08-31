using PerformanceDataExtractor.DTOs;

namespace PerformanceDataExtractor.Models;

public class WeeklyReportData
{
    public DateTime WeekStartDate { get; set; }
    public DateTime WeekEndDate { get; set; }
    public int TotalTests { get; set; }
    public double AverageResponseTime { get; set; }
    public double AverageThroughput { get; set; }
    public double AverageErrorRate { get; set; }
    public List<WeeklyTestSummary> TestSummaries { get; set; } = new();
}
