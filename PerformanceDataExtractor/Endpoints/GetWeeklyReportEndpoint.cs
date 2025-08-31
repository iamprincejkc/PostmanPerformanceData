using FastEndpoints;
using PerformanceDataExtractor.DTOs;
using PerformanceDataExtractor.Services;

namespace PerformanceDataExtractor.Endpoints;

public class GetWeeklyReportEndpoint : EndpointWithoutRequest<GetWeeklyReportResponse>
{
    private readonly IPerformanceDataService _performanceDataService;

    public GetWeeklyReportEndpoint(IPerformanceDataService performanceDataService)
    {
        _performanceDataService = performanceDataService;
    }

    public override void Configure()
    {
        Get("/api/weekly-report");
        AllowAnonymous();
        Description(b => b
            .WithTags("Reports")
            .WithSummary("Get weekly performance report")
            .WithDescription("Gets performance data for the current week"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            // Get current week's start and end dates
            var now = DateTime.UtcNow;
            var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var reportData = await _performanceDataService.GetWeeklyReportDataAsync(startOfWeek, endOfWeek);

            await SendOkAsync(new GetWeeklyReportResponse
            {
                WeekStartDate = reportData.WeekStartDate,
                WeekEndDate = reportData.WeekEndDate,
                TotalTests = reportData.TotalTests,
                AverageResponseTime = reportData.AverageResponseTime,
                AverageThroughput = reportData.AverageThroughput,
                AverageErrorRate = reportData.AverageErrorRate,
                TestSummaries = reportData.TestSummaries
            }, ct);
        }
        catch (Exception ex)
        {
            await SendErrorsAsync(500, ct);
            Logger.LogError(ex, "Error generating weekly report");
        }
    }
}
