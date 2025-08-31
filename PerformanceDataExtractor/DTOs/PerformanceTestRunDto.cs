namespace PerformanceDataExtractor.DTOs;

public class PerformanceTestRunDto
{
    public string TestName { get; set; } = string.Empty;
    public string TestId { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int TotalRequests { get; set; }
    public double Throughput { get; set; }
    public double AverageResponseTime { get; set; }
    public double ErrorRate { get; set; }
    public int VirtualUsers { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string LoadProfile { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public List<RequestMetricDto> RequestMetrics { get; set; } = new();
}
