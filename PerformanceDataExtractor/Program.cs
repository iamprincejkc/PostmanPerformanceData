using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using PerformanceDataExtractor.Data;
using PerformanceDataExtractor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PerformanceDbContext>(options =>
    options.UseInMemoryDatabase("PerformanceTestDb"));

builder.Services.AddScoped<IPerformanceDataService, PerformanceDataService>();
builder.Services.AddScoped<IEnhancedPerformanceDataService, EnhancedPerformanceDataService>();

builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Performance Data Extractor API";
            s.Version = "v1";
            s.Description = "API for managing Postman performance test data";
        };
    });


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseCors("AllowAll");

app.UseFastEndpoints()
.UseSwaggerGen();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PerformanceDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
