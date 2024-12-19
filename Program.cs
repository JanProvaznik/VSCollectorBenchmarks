using System;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.VisualStudio.OpenTelemetry.ClientExtensions;
using Microsoft.VisualStudio.OpenTelemetry.ClientExtensions.Exporters;
using Microsoft.VisualStudio.OpenTelemetry.Collector.Interfaces;
using Microsoft.VisualStudio.OpenTelemetry.Collector.Settings;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;

public class CollectorBenchmarks
{
    private const string ActivitySourceName = "MSBuild";
    private static readonly ActivitySource _activitySource = new ActivitySource(ActivitySourceName);

    [GlobalSetup]
    public void Setup()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
    }

    [Benchmark(Baseline = true)]
    public void NoCollector()
    {
        Activity activity = null;
        try
        {
            activity = _activitySource.StartActivity("NoCollector");
            if (activity != null)
            {
                activity.SetTag("benchmark", "baseline");
            }
        }
        finally
        {
            activity?.Dispose();
        }
    }

    [Benchmark]
    public void StartTracerProvider()
    {
        var exporterSettings = OpenTelemetryExporterSettingsBuilder
            .CreateVSDefault("17.0")
            .Build();

        TracerProvider tracerProvider = null;
        Activity activity = null;
        try
        {
            tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySourceName)
                .AddVisualStudioDefaultTraceExporter(exporterSettings)
                .Build();

            activity = _activitySource.StartActivity("TracerProvider");
            if (activity != null)
            {
                activity.SetTag("benchmark", "tracerProvider");
            }
        }
        finally
        {
            activity?.Dispose();
            tracerProvider?.Dispose();
        }
    }

    [Benchmark]
    public void StartCollectorStandalone()
    {
        var exporterSettings = OpenTelemetryExporterSettingsBuilder
            .CreateVSDefault("17.0")
            .Build();

        TracerProvider tracerProvider = null;
        IOpenTelemetryCollector collector = null;
        Activity activity = null;
        try
        {
            tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(ActivitySourceName)
                .AddVisualStudioDefaultTraceExporter(exporterSettings)
                .Build();

            var collectorSettings = OpenTelemetryCollectorSettingsBuilder
                .CreateVSDefault("17.0")
                .Build();

            collector = OpenTelemetryCollectorProvider
                .CreateCollector(collectorSettings);
            collector.StartAsync().Wait();

            activity = _activitySource.StartActivity("Collector");
            if (activity != null)
            {
                activity.SetTag("benchmark", "collector");
            }
        }
        finally
        {
            activity?.Dispose();
            collector?.Dispose();
            tracerProvider?.Dispose();
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<CollectorBenchmarks>();
    }
}
