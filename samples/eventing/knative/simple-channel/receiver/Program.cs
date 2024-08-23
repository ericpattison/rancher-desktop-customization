using CloudNative.CloudEvents.AspNetCore;
using CloudNative.CloudEvents.SystemTextJson;
using CloudNative.CloudEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/receive/message", async (IHttpContextAccessor contextAccessor) =>
{
    CloudEventFormatter formatter = new JsonEventFormatter<Message>();
    CloudEvent cloudEvent = await contextAccessor.HttpContext.Request.ToCloudEventAsync(formatter);
    Message message = (Message)cloudEvent.Data;

    Console.WriteLine($"Message: {message.Text}");
})
.WithName("ReceiveMessage")
.WithOpenApi();

app.Run();

class Message {
    public string Text{get;set;}
}