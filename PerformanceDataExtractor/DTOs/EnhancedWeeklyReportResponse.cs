namespace PerformanceDataExtractor.DTOs;
public class EnhancedWeeklyReportResponse
{
    public DateTime WeekStartDate { get; set; }
    public DateTime WeekEndDate { get; set; }
    public int TotalTests { get; set; }

    // Main metrics with trends
    public MetricWithTrend AverageResponseTime { get; set; } = new();
    public MetricWithTrend AverageThroughput { get; set; } = new();
    public MetricWithTrend AverageErrorRate { get; set; } = new();
    public MetricWithTrend TotalRequestsProcessed { get; set; } = new();

    // Performance indicators
    public PerformanceIndicators Indicators { get; set; } = new();

    // Test summaries
    public List<WeeklyTestSummary> TestSummaries { get; set; } = new();

    // Chart data
    public ChartData Charts { get; set; } = new();

    // Best and worst performing requests
    public List<TopPerformingRequest> FastestRequests { get; set; } = new();
    public List<TopPerformingRequest> SlowestRequests { get; set; } = new();
}

public class MetricWithTrend
{
    public double CurrentValue { get; set; }
    public double PreviousValue { get; set; }
    public double PercentageChange { get; set; }
    public TrendDirection Trend { get; set; }
    public string FormattedValue { get; set; } = string.Empty;
    public string FormattedChange { get; set; } = string.Empty;
}

public class PerformanceIndicators
{
    public string OverallHealthStatus { get; set; } = string.Empty; // "Excellent", "Good", "Warning", "Critical"
    public double PerformanceScore { get; set; } // 0-100 score
    public List<string> Insights { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
}

public class ChartData
{
    public List<string> TimeLabels { get; set; } = new();
    public List<double> ResponseTimeData { get; set; } = new();
    public List<double> ThroughputData { get; set; } = new();
    public List<double> ErrorRateData { get; set; } = new();
    public List<double> TotalRequestsData { get; set; } = new();
}

public class TopPerformingRequest
{
    public string RequestName { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;
    public double AverageResponseTime { get; set; }
    public int TotalRequests { get; set; }
    public double ErrorRate { get; set; }
}

public enum TrendDirection
{
    Up,
    Down,
    Stable,
    NoData
}