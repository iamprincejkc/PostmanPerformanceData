namespace PerformanceDataExtractor.DTOs;

public class RequestMetricDto
{
    public string RequestName { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int TotalRequests { get; set; }
    public double RequestsPerSecond { get; set; }
    public int MinResponseTime { get; set; }
    public int AvgResponseTime { get; set; }
    public int NinetiethPercentile { get; set; }
    public int MaxResponseTime { get; set; }
    public double ErrorPercentage { get; set; }
}
