using GameQuiz.Application.Interfaces;
using GameQuiz.Application.Services;
using GameQuiz.Domain.Interfaces;
using GameQuiz.Infrastructure.Data;
using GameQuiz.Infrastructure.Repositories;
using GameQuiz.WebApi;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Serilog;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();

builder.Host.UseSerilog((context, services, config) =>
{
    var aiConfig = services.GetRequiredService<TelemetryConfiguration>();
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.With<ActivityEnricher>()
        .WriteTo.ApplicationInsights(aiConfig, TelemetryConverter.Traces);
});

builder.Services.AddControllers();
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
        var activity = Activity.Current;
        if (activity != null)
        {
            context.ProblemDetails.Extensions.TryAdd("traceId", activity.Id);
        }
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameQuizDB"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddHybridCache(options => 
    options.DefaultEntryOptions = new HybridCacheEntryOptions 
    { 
        LocalCacheExpiration = TimeSpan.FromMinutes(5) 
    });

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
