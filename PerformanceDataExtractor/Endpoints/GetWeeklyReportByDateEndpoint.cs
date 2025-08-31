using FastEndpoints;
using Microsoft.Extensions.Logging;
using PerformanceDataExtractor.DTOs;
using PerformanceDataExtractor.Services;

namespace PerformanceDataExtractor.Endpoints;
public class GetWeeklyReportByDateRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class GetWeeklyReportByDateEndpoint : Endpoint<GetWeeklyReportByDateRequest, GetWeeklyReportResponse>
{
    private readonly IPerformanceDataService _performanceDataService;

    public GetWeeklyReportByDateEndpoint(IPerformanceDataService performanceDataService)
    {
        _performanceDataService = performanceDataService;
    }

    public override void Configure()
    {
        Get("/api/weekly-report/{startDate}/{endDate}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Reports")
            .WithSummary("Get weekly performance report by date range")
            .WithDescription("Gets performance data for a specific date range"));
    }

    public override async Task HandleAsync(GetWeeklyReportByDateRequest req, CancellationToken ct)
    {
        try
        {
            var reportData = await _performanceDataService.GetWeeklyReportDataAsync(req.StartDate, req.EndDate);

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
            Logger.LogError(ex, "Error generating weekly report for date range");
        }
    }
}