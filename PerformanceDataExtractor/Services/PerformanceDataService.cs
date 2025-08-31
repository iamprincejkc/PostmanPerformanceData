using Microsoft.EntityFrameworkCore;
using PerformanceDataExtractor.Data;
using PerformanceDataExtractor.DTOs;
using PerformanceDataExtractor.Models;

namespace PerformanceDataExtractor.Services;

public class PerformanceDataService : IPerformanceDataService
{
    private readonly PerformanceDbContext _context;

    public PerformanceDataService(PerformanceDbContext context)
    {
        _context = context;
    }

    public async Task<int> SavePerformanceTestRunAsync(PerformanceTestRunDto testRunDto)
    {
        var testRun = new PerformanceTestRun
        {
            TestName = testRunDto.TestName,
            TestId = testRunDto.TestId,
            StartTime = testRunDto.StartTime,
            EndTime = testRunDto.EndTime,
            TotalRequests = testRunDto.TotalRequests,
            Throughput = testRunDto.Throughput,
            AverageResponseTime = testRunDto.AverageResponseTime,
            ErrorRate = testRunDto.ErrorRate,
            VirtualUsers = testRunDto.VirtualUsers,
            Duration = testRunDto.Duration,
            LoadProfile = testRunDto.LoadProfile,
            Environment = testRunDto.Environment,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var metricDto in testRunDto.RequestMetrics)
        {
            testRun.RequestMetrics.Add(new RequestMetric
            {
                RequestName = metricDto.RequestName,
                HttpMethod = metricDto.HttpMethod,
                Url = metricDto.Url,
                TotalRequests = metricDto.TotalRequests,
                RequestsPerSecond = metricDto.RequestsPerSecond,
                MinResponseTime = metricDto.MinResponseTime,
                AvgResponseTime = metricDto.AvgResponseTime,
                NinetiethPercentile = metricDto.NinetiethPercentile,
                MaxResponseTime = metricDto.MaxResponseTime,
                ErrorPercentage = metricDto.ErrorPercentage
            });
        }

        _context.PerformanceTestRuns.Add(testRun);
        await _context.SaveChangesAsync();

        return testRun.Id;
    }

    public async Task<List<PerformanceTestRun>> GetWeeklyTestRunsAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.PerformanceTestRuns
            .Include(t => t.RequestMetrics)
            .Where(t => t.StartTime >= startDate && t.StartTime < endDate)
            .OrderBy(t => t.StartTime)
            .ToListAsync();
    }

    public async Task<WeeklyReportData> GetWeeklyReportDataAsync(DateTime startDate, DateTime endDate)
    {

        var testRuns = await _context.PerformanceTestRuns
            .Include(t => t.RequestMetrics)
            .Where(t => (t.StartTime >= startDate.AddDays(-1) && t.StartTime < endDate) ||
                       (t.CreatedAt >= startDate && t.CreatedAt < endDate))
            .OrderBy(t => t.StartTime)
            .ToListAsync();

        Console.WriteLine($"Found {testRuns.Count} test runs in date range");

        if (!testRuns.Any())
        {
            return new WeeklyReportData
            {
                WeekStartDate = startDate,
                WeekEndDate = endDate,
                TotalTests = 0,
                AverageResponseTime = 0,
                AverageThroughput = 0,
                AverageErrorRate = 0,
                TestSummaries = new List<WeeklyTestSummary>()
            };
        }

        return new WeeklyReportData
        {
            WeekStartDate = startDate,
            WeekEndDate = endDate,
            TotalTests = testRuns.Count,
            AverageResponseTime = testRuns.Average(t => t.AverageResponseTime),
            AverageThroughput = testRuns.Average(t => t.Throughput),
            AverageErrorRate = testRuns.Average(t => t.ErrorRate),
            TestSummaries = testRuns.Select(t => new WeeklyTestSummary
            {
                TestName = t.TestName,
                TestDate = t.StartTime,
                TotalRequests = t.TotalRequests,
                Throughput = t.Throughput,
                AverageResponseTime = t.AverageResponseTime,
                ErrorRate = t.ErrorRate,
                RequestTypesCount = t.RequestMetrics.Count
            }).ToList()
        };
    }
}
