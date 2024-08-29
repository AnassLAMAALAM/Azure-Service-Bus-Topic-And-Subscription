using Microsoft.Azure.ServiceBus;
using Publisher.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register a specific instance of ITopicClient
builder.Services.AddTransient<ITopicClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("AzureServiceBus");
    var topicName = "anass-topic"; // Replace with your actual topic name

    return new TopicClient(connectionString, topicName);
});

// Register the ServiceBusService with dependency injection
builder.Services.AddTransient<IServiceBusService, ServiceBusService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
