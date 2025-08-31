using PerformanceDataExtractor.DTOs;
using PerformanceDataExtractor.Models;

namespace PerformanceDataExtractor.Services;

public interface IPerformanceDataService
{
    Task<int> SavePerformanceTestRunAsync(PerformanceTestRunDto testRunDto);
    Task<List<PerformanceTestRun>> GetWeeklyTestRunsAsync(DateTime startDate, DateTime endDate);
    Task<WeeklyReportData> GetWeeklyReportDataAsync(DateTime startDate, DateTime endDate);
}
