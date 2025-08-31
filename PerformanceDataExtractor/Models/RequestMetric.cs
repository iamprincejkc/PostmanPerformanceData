using System.ComponentModel.DataAnnotations;

namespace PerformanceDataExtractor.Models;

public class RequestMetric
{
    public int Id { get; set; }

    public int PerformanceTestRunId { get; set; }

    public PerformanceTestRun PerformanceTestRun { get; set; } = null!;

    [Required]
    public string RequestName { get; set; } = string.Empty;

    [Required]
    public string HttpMethod { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;

    public int TotalRequests { get; set; }

    public double RequestsPerSecond { get; set; }

    public int MinResponseTime { get; set; }

    public int AvgResponseTime { get; set; }

    public int NinetiethPercentile { get; set; }

    public int MaxResponseTime { get; set; }

    public double ErrorPercentage { get; set; }
}
