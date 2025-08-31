using PerformanceDataExtractor.DTOs;

namespace PerformanceDataExtractor.Services;

public interface IEnhancedPerformanceDataService
{
    Task<EnhancedWeeklyReportResponse> GetEnhancedWeeklyReportAsync(DateTime startDate, DateTime endDate);
}
