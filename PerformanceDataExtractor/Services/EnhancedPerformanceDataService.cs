using PerformanceDataExtractor.Data;
using PerformanceDataExtractor.DTOs;
using PerformanceDataExtractor.Models;

namespace PerformanceDataExtractor.Services;

public class EnhancedPerformanceDataService : PerformanceDataService, IEnhancedPerformanceDataService
{
    public EnhancedPerformanceDataService(PerformanceDbContext context) : base(context)
    {
    }

    public async Task<EnhancedWeeklyReportResponse> GetEnhancedWeeklyReportAsync(DateTime startDate, DateTime endDate)
    {
        // Get current week data
        var currentWeekTests = await GetWeeklyTestRunsAsync(startDate, endDate);

        // Get previous week data for comparison
        var previousWeekStart = startDate.AddDays(-7);
        var previousWeekEnd = endDate.AddDays(-7);
        var previousWeekTests = await GetWeeklyTestRunsAsync(previousWeekStart, previousWeekEnd);

        // Calculate metrics with trends
        var responseTimeMetric = CalculateMetricWithTrend(
            currentWeekTests.Any() ? currentWeekTests.Average(t => t.AverageResponseTime) : 0,
            previousWeekTests.Any() ? previousWeekTests.Average(t => t.AverageResponseTime) : 0,
            "ms");

        var throughputMetric = CalculateMetricWithTrend(
            currentWeekTests.Any() ? currentWeekTests.Average(t => t.Throughput) : 0,
            previousWeekTests.Any() ? previousWeekTests.Average(t => t.Throughput) : 0,
            "req/s");

        var errorRateMetric = CalculateMetricWithTrend(
            currentWeekTests.Any() ? currentWeekTests.Average(t => t.ErrorRate) : 0,
            previousWeekTests.Any() ? previousWeekTests.Average(t => t.ErrorRate) : 0,
            "%");

        var totalRequestsMetric = CalculateMetricWithTrend(
            currentWeekTests.Sum(t => t.TotalRequests),
            previousWeekTests.Sum(t => t.TotalRequests),
            "requests");

        // Calculate performance indicators
        var indicators = CalculatePerformanceIndicators(currentWeekTests, responseTimeMetric, errorRateMetric);

        // Generate chart data
        var chartData = GenerateChartData(currentWeekTests);

        // Get top performing requests
        var allRequests = currentWeekTests.SelectMany(t => t.RequestMetrics).ToList();
        var fastestRequests = GetTopRequests(allRequests, true);
        var slowestRequests = GetTopRequests(allRequests, false);

        return new EnhancedWeeklyReportResponse
        {
            WeekStartDate = startDate,
            WeekEndDate = endDate,
            TotalTests = currentWeekTests.Count,
            AverageResponseTime = responseTimeMetric,
            AverageThroughput = throughputMetric,
            AverageErrorRate = errorRateMetric,
            TotalRequestsProcessed = totalRequestsMetric,
            Indicators = indicators,
            TestSummaries = currentWeekTests.Select(t => new WeeklyTestSummary
            {
                TestName = t.TestName,
                TestDate = t.StartTime,
                TotalRequests = t.TotalRequests,
                Throughput = t.Throughput,
                AverageResponseTime = t.AverageResponseTime,
                ErrorRate = t.ErrorRate,
                RequestTypesCount = t.RequestMetrics.Count
            }).ToList(),
            Charts = chartData,
            FastestRequests = fastestRequests,
            SlowestRequests = slowestRequests
        };
    }

    private MetricWithTrend CalculateMetricWithTrend(double currentValue, double previousValue, string unit)
    {
        var percentageChange = previousValue > 0 ? ((currentValue - previousValue) / previousValue) * 100 : 0;
        var trend = Math.Abs(percentageChange) < 1 ? TrendDirection.Stable :
                   percentageChange > 0 ? TrendDirection.Up : TrendDirection.Down;

        return new MetricWithTrend
        {
            CurrentValue = currentValue,
            PreviousValue = previousValue,
            PercentageChange = percentageChange,
            Trend = trend,
            FormattedValue = $"{currentValue:F2} {unit}",
            FormattedChange = $"{(percentageChange >= 0 ? "+" : "")}{percentageChange:F1}%"
        };
    }

    private PerformanceIndicators CalculatePerformanceIndicators(List<PerformanceTestRun> tests, MetricWithTrend responseTime, MetricWithTrend errorRate)
    {
        var insights = new List<string>();
        var recommendations = new List<string>();
        var score = 100.0;

        // Calculate performance score and generate insights
        if (responseTime.CurrentValue > 1000)
        {
            score -= 20;
            insights.Add("High response times detected (>1000ms)");
            recommendations.Add("Investigate slow endpoints and optimize database queries");
        }

        if (errorRate.CurrentValue > 1)
        {
            score -= 30;
            insights.Add($"Error rate above 1% ({errorRate.CurrentValue:F2}%)");
            recommendations.Add("Review failed requests and implement error handling");
        }

        if (responseTime.Trend == TrendDirection.Up)
        {
            score -= 10;
            insights.Add($"Response times trending upward ({responseTime.FormattedChange})");
            recommendations.Add("Monitor system resources and consider scaling");
        }

        if (tests.Any() && tests.Average(t => t.Throughput) < 1)
        {
            score -= 15;
            insights.Add("Low throughput detected (<1 req/s)");
            recommendations.Add("Consider load balancing or infrastructure improvements");
        }

        var healthStatus = score >= 90 ? "Excellent" :
                          score >= 75 ? "Good" :
                          score >= 60 ? "Warning" : "Critical";

        if (!insights.Any())
        {
            insights.Add("All performance metrics within acceptable ranges");
        }

        if (!recommendations.Any())
        {
            recommendations.Add("Continue monitoring performance trends");
        }

        return new PerformanceIndicators
        {
            OverallHealthStatus = healthStatus,
            PerformanceScore = Math.Max(0, score),
            Insights = insights,
            Recommendations = recommendations
        };
    }

    private ChartData GenerateChartData(List<PerformanceTestRun> tests)
    {
        var sortedTests = tests.OrderBy(t => t.StartTime).ToList();

        return new ChartData
        {
            TimeLabels = sortedTests.Select(t => t.StartTime.ToString("MM/dd HH:mm")).ToList(),
            ResponseTimeData = sortedTests.Select(t => t.AverageResponseTime).ToList(),
            ThroughputData = sortedTests.Select(t => t.Throughput).ToList(),
            ErrorRateData = sortedTests.Select(t => t.ErrorRate).ToList(),
            TotalRequestsData = sortedTests.Select(t => (double)t.TotalRequests).ToList()
        };
    }

    private List<TopPerformingRequest> GetTopRequests(List<RequestMetric> allRequests, bool fastest)
    {
        var grouped = allRequests
            .GroupBy(r => new { r.RequestName, r.HttpMethod })
            .Select(g => new TopPerformingRequest
            {
                RequestName = g.Key.RequestName,
                HttpMethod = g.Key.HttpMethod,
                AverageResponseTime = g.Average(r => r.AvgResponseTime),
                TotalRequests = g.Sum(r => r.TotalRequests),
                ErrorRate = g.Average(r => r.ErrorPercentage)
            });

        return fastest
            ? grouped.OrderBy(r => r.AverageResponseTime).Take(5).ToList()
            : grouped.OrderByDescending(r => r.AverageResponseTime).Take(5).ToList();
    }
}