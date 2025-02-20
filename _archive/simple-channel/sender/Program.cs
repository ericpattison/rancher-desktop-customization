using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Http;
using CloudNative.CloudEvents.SystemTextJson;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddOptions();
builder.Services.Configure<EventChannel>(builder.Configuration.GetSection(nameof(EventChannel)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/send/message", async (Message msg, IHttpClientFactory clientFactory, IOptions<EventChannel> eventChannelOptions) =>
{
    CloudEvent cloudEvent = new() {
        Id = Guid.NewGuid().ToString(),
        Type = "send-message",
        Source = new Uri("https://coudevents.io"),
        Time = DateTimeOffset.UtcNow,
        DataContentType = "application/json",
        Data = msg,
    };

    JsonEventFormatter formatter = new();
    using HttpClient client = clientFactory.CreateClient();
    client.BaseAddress = new System.Uri(eventChannelOptions.Value.BaseUrl);
    var content = cloudEvent.ToHttpContent(ContentMode.Structured, formatter);
    var result = await client.PostAsync("", content);
})
.WithName("SendMessage")
.WithOpenApi();

app.Run();


class Message {
    public string Text{get;set;}
}
class EventChannel {
    public string BaseUrl{get;set;}
}