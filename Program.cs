using Microsoft.Azure.Cosmos;
using AzureCosmosCRUDWebAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Cosmos DB Configuration
var cosmosDbConfig = builder.Configuration.GetSection("CosmosDb");
builder.Services.AddSingleton<CosmosClient>(s =>
    new CosmosClient(cosmosDbConfig["AccountEndpoint"], cosmosDbConfig["AccountKey"]));
builder.Services.AddSingleton<CosmosDbService>(s =>
    new CosmosDbService(s.GetRequiredService<CosmosClient>(), cosmosDbConfig["DatabaseName"], cosmosDbConfig["ContainerName"]));

// Add other necessary services like Swagger if needed
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(a => a.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature?.Error;

    var result = JsonConvert.SerializeObject(new { error = exception?.Message });
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(result);
}));


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
