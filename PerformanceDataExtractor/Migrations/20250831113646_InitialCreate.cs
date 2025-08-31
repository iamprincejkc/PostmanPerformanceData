using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerformanceDataExtractor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerformanceTestRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalRequests = table.Column<int>(type: "int", nullable: false),
                    Throughput = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: false),
                    AverageResponseTime = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: false),
                    ErrorRate = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false),
                    VirtualUsers = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LoadProfile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Environment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceTestRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformanceTestRunId = table.Column<int>(type: "int", nullable: false),
                    RequestName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HttpMethod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    TotalRequests = table.Column<int>(type: "int", nullable: false),
                    RequestsPerSecond = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: false),
                    MinResponseTime = table.Column<int>(type: "int", nullable: false),
                    AvgResponseTime = table.Column<int>(type: "int", nullable: false),
                    NinetiethPercentile = table.Column<int>(type: "int", nullable: false),
                    MaxResponseTime = table.Column<int>(type: "int", nullable: false),
                    ErrorPercentage = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestMetrics_PerformanceTestRuns_PerformanceTestRunId",
                        column: x => x.PerformanceTestRunId,
                        principalTable: "PerformanceTestRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceTestRuns_CreatedAt",
                table: "PerformanceTestRuns",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceTestRuns_StartTime",
                table: "PerformanceTestRuns",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceTestRuns_TestName",
                table: "PerformanceTestRuns",
                column: "TestName");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMetrics_AvgResponseTime",
                table: "RequestMetrics",
                column: "AvgResponseTime");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMetrics_HttpMethod",
                table: "RequestMetrics",
                column: "HttpMethod");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMetrics_PerformanceTestRunId",
                table: "RequestMetrics",
                column: "PerformanceTestRunId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMetrics_RequestName",
                table: "RequestMetrics",
                column: "RequestName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestMetrics");

            migrationBuilder.DropTable(
                name: "PerformanceTestRuns");
        }
    }
}
