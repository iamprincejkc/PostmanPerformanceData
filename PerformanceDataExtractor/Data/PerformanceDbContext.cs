// Data/PerformanceDbContext.cs
using Microsoft.EntityFrameworkCore;
using PerformanceDataExtractor.DTOs;
using PerformanceDataExtractor.Models;

namespace PerformanceDataExtractor.Data;

public class PerformanceDbContext : DbContext
{
    public PerformanceDbContext(DbContextOptions<PerformanceDbContext> options) : base(options)
    {
    }

    public DbSet<PerformanceTestRun> PerformanceTestRuns { get; set; }
    public DbSet<RequestMetric> RequestMetrics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PerformanceTestRun>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TestName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.TestId).HasMaxLength(100);
            entity.Property(e => e.Duration).HasMaxLength(100);
            entity.Property(e => e.LoadProfile).HasMaxLength(100);
            entity.Property(e => e.Environment).HasMaxLength(100);
            entity.Property(e => e.Throughput).HasPrecision(10, 2);
            entity.Property(e => e.AverageResponseTime).HasPrecision(10, 2);
            entity.Property(e => e.ErrorRate).HasPrecision(5, 2);

            entity.HasIndex(e => e.TestName);
            entity.HasIndex(e => e.StartTime);
            entity.HasIndex(e => e.CreatedAt);
        });

        modelBuilder.Entity<RequestMetric>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.HttpMethod).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.RequestsPerSecond).HasPrecision(10, 2);
            entity.Property(e => e.ErrorPercentage).HasPrecision(5, 2);

            entity.HasOne(e => e.PerformanceTestRun)
                  .WithMany(e => e.RequestMetrics)
                  .HasForeignKey(e => e.PerformanceTestRunId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.RequestName);
            entity.HasIndex(e => e.HttpMethod);
            entity.HasIndex(e => e.AvgResponseTime);
        });
    }
}
