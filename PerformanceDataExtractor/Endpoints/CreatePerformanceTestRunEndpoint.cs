using FastEndpoints;
using Microsoft.Extensions.Logging;
using PerformanceDataExtractor.DTOs;
using PerformanceDataExtractor.Services;

namespace PerformanceDataExtractor.Endpoints;
public class CreatePerformanceTestRunEndpoint : Endpoint<CreatePerformanceTestRunRequest, CreatePerformanceTestRunResponse>
{
    private readonly IPerformanceDataService _performanceDataService;

    public CreatePerformanceTestRunEndpoint(IPerformanceDataService performanceDataService)
    {
        _performanceDataService = performanceDataService;
    }

    public override void Configure()
    {
        Post("/api/performance-tests");
        AllowAnonymous();
        Description(b => b
            .WithTags("Performance Tests")
            .WithSummary("Create a new performance test run")
            .WithDescription("Creates a new performance test run with request metrics"));
    }

    public override async Task HandleAsync(CreatePerformanceTestRunRequest req, CancellationToken ct)
    {
        try
        {
            var id = await _performanceDataService.SavePerformanceTestRunAsync(req.TestRun);

            await SendOkAsync(new CreatePerformanceTestRunResponse
            {
                Id = id,
                Message = $"Performance test run created successfully with ID: {id}"
            }, ct);
        }
        catch (Exception ex)
        {
            await SendErrorsAsync(500, ct);
            Logger.LogError(ex, "Error creating performance test run");
        }
    }
}