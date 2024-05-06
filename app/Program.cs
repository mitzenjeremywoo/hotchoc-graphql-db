using app;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddGraphQLServer()
//    .AddQueryType<Query>();

builder.Services
    .AddSingleton(sp =>
    {
        const string connectionString = "mongodb+srv://username:password@cluster0.psk0yd7.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

        var mongoConnectionUrl = new MongoUrl(connectionString);

        var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);

        mongoClientSettings.ClusterConfigurator = cb =>
        {
            // This will print the executed command to the console
            cb.Subscribe<CommandStartedEvent>(e =>
            {
                Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
            });
        };
        var client = new MongoClient(mongoClientSettings);
        var database = client.GetDatabase("mydemo");
        return database.GetCollection<Restaurant>("todo");
    })
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutations>()
    .AddGlobalObjectIdentification()
    // Registers the filter convention of MongoDB
    .AddMongoDbFiltering()
    // Registers the sorting convention of MongoDB
    .AddMongoDbSorting()
    // Registers the projection convention of MongoDB
    .AddMongoDbProjections()
    // Registers the paging providers of MongoDB
    .AddMongoDbPagingProviders();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGraphQL();

app.UseHttpsRedirection();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
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
