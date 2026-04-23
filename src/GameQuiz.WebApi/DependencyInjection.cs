using GameQuiz.WebApi.Exceptions;
using GameQuiz.WebApi.Logging;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Caching.Hybrid;
using Serilog;
using System.Diagnostics;

namespace GameQuiz.WebApi;

internal static class DependencyInjection
{
    public static IServiceCollection AddApi(this WebApplicationBuilder builder)
    {
        return builder
            .AddTelemetryAndLogging()
            .AddGlobalExceptionHandling()
            .AddHybridCaching()
            .AddOpenApi();
    }

    private static IServiceCollection AddTelemetryAndLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationInsightsTelemetry();
        builder.Host.UseSerilog((context, services, config) =>
        {
            var aiConfig = services.GetRequiredService<TelemetryConfiguration>();
            config
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.With<ActivityEnricher>()
                .WriteTo.ApplicationInsights(aiConfig, TelemetryConverter.Traces);
        });
        return builder.Services;
    }

    private static IServiceCollection AddGlobalExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails(options =>
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
        return services;
    }
        
    private static IServiceCollection AddHybridCaching(this IServiceCollection services)
    {
        services.AddHybridCache(options =>
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            });
        return services;
    }
}