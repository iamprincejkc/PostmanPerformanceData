using System.ComponentModel.DataAnnotations;

namespace PerformanceDataExtractor.Models;

public class PerformanceTestRun
{
    public int Id { get; set; }

    [Required]
    public string TestName { get; set; } = string.Empty;

    public string TestId { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TotalRequests { get; set; }

    public double Throughput { get; set; }

    public double AverageResponseTime { get; set; }

    public double ErrorRate { get; set; }

    public int VirtualUsers { get; set; }

    public string Duration { get; set; } = string.Empty;

    public string LoadProfile { get; set; } = string.Empty;

    public string Environment { get; set; } = string.Empty;

    public List<RequestMetric> RequestMetrics { get; set; } = new();
}