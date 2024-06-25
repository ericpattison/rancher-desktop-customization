using Client;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ISvc2, Svc2>();

const string ServiceName = "SVC1";
Uri OTLP_URI = new Uri("http://otel-lgtm.monitoring.svc.cluster.local:4317");

builder.Logging.AddOpenTelemetry(options => {
    options.SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(ServiceName)
    ).AddConsoleExporter()
    .AddOtlpExporter(options => options.Endpoint = OTLP_URI);
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(ServiceName))
    .WithTracing(tracing => {
        tracing.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter(options => options.Endpoint = OTLP_URI);
    })
    .WithMetrics(metrics => {
        metrics.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter(options => options.Endpoint = OTLP_URI);
    });


var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/orders/{id}", async (int id, [FromServices]ISvc2 client) =>
{
    return await client.GetOrderById(id);
})
.WithName("GetOrderById");

app.Run();
