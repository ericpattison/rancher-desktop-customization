using classicmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ClassicModelsContext>(options=> {
    var connectionString = builder.Configuration.GetConnectionString("ClassicModels");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

const string ServiceName = "SVC2";
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
            .AddEntityFrameworkCoreInstrumentation()
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

app.MapGet("/orders/{id}", async (int id, [FromServices]ClassicModelsContext context) =>
{
    var orders = await context.Orders.Where(o => o.OrderNumber == id).ToListAsync();
    return orders.FirstOrDefault();
})
.WithName("GetOrderById");

app.Run();
