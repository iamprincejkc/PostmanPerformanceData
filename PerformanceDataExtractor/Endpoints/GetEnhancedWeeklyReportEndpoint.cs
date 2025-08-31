using FastEndpoints;
using Microsoft.Extensions.Logging;
using PerformanceDataExtractor.DTOs;
using PerformanceDataExtractor.Services;

namespace PerformanceDataExtractor.Endpoints;

public class GetEnhancedWeeklyReportEndpoint : EndpointWithoutRequest<EnhancedWeeklyReportResponse>
{
    private readonly IEnhancedPerformanceDataService _performanceDataService;

    public GetEnhancedWeeklyReportEndpoint(IEnhancedPerformanceDataService performanceDataService)
    {
        _performanceDataService = performanceDataService;
    }

    public override void Configure()
    {
        Get("/api/enhanced-weekly-report");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get enhanced weekly performance report with trends";
            s.Description = "Gets detailed performance analytics with trend analysis and charts";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var now = DateTime.UtcNow;
            var startOfWeek = now.AddDays(-(int)now.DayOfWeek).Date.AddDays(-1); // Your fix for date
            var endOfWeek = startOfWeek.AddDays(7);

            Logger.LogInformation($"Fetching enhanced weekly report from {startOfWeek:yyyy-MM-dd} to {endOfWeek:yyyy-MM-dd}");

            var reportData = await _performanceDataService.GetEnhancedWeeklyReportAsync(startOfWeek, endOfWeek);

            await SendOkAsync(reportData, ct);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error generating enhanced weekly report");
            await SendErrorsAsync(500, ct);
        }
    }
}