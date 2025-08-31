namespace PerformanceDataExtractor.DTOs;

public class CreatePerformanceTestRunRequest
{
    public PerformanceTestRunDto TestRun { get; set; } = new();
}
