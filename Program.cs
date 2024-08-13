using MongoDB.Driver;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Retrieve MongoDB connection string from appsettings.json
var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings");
var connectionUri = mongoDBSettings.GetValue<string>("ConnectionString");
var databaseName = mongoDBSettings.GetValue<string>("DatabaseName");

var settings = MongoClientSettings.FromConnectionString(connectionUri);
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);

// Register MongoClient and IMongoDatabase with the DI container
builder.Services.AddSingleton<IMongoClient>(client);
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    client.GetDatabase(databaseName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ping MongoDB to confirm the connection
try
{
    var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
    Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
