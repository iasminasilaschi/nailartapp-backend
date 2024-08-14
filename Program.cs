using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Retrieve MongoDB connection string and database name from appsettings.json
var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings");
var connectionUri = mongoDBSettings.GetValue<string>("ConnectionString");
var databaseName = mongoDBSettings.GetValue<string>("DatabaseName");

// Set up MongoDB client settings
var settings = MongoClientSettings.FromConnectionString(connectionUri);
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);

// Register MongoClient and IMongoDatabase with the DI container
builder.Services.AddSingleton<IMongoClient>(client);
builder.Services.AddSingleton<IMongoDatabase>(sp => client.GetDatabase(databaseName));

// Register UserService with the DI container
builder.Services.AddSingleton<UserService>();

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
    Console.WriteLine($"Error connecting to MongoDB: {ex.Message}");
}

// Define an API endpoint to create a new user
app.MapPost("/create-user", (UserService userService, string username, string password, string role) =>
{
    var result = userService.CreateUser(username, password, role);
    if (!result.Success)
    {
        return Results.BadRequest(result.Message);
    }
    return Results.Ok(result.Message);
});


app.Run();
