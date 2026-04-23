using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace GameQuiz.WebApi.Logging;

internal class ActivityEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory factory)
    {
        var activity = Activity.Current;
        if (activity != null)
        {
            logEvent.AddPropertyIfAbsent(factory.CreateProperty("TraceId", activity.Id));
        }
    }
}
