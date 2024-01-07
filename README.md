# How to create a .NET8 WebAPI CRUD Azure CosmosDB Microservice

The code for this example is available in this github repo: https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB

## 1. Create Azure CosmosDB

We navigate to **Azure CosmosDB** service and we click in this option to create a new account:

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/3822db85-173d-411d-8383-7f97d07c05f4)

We click on **Azure CosmosDB account** button

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/c1080788-bfc0-445e-9084-ce0f0772044a)

Now we select the option **Azure Cosmos DB for NoSQL**, and we press the **create** button

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/69182e3f-9493-41a8-ab6b-92308a9bdddf)

In following screen we input the required data for creating the service

We create a new **ResourceGroup name**: myRG

We set the **account name**: mycosmosdbluis1974

We choose the service **location**: France Central

Capacity mode: **serverless**

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/ed5a1407-0303-44b3-b528-7a7fc1398d80)

We navigate to the **Data Explorer** page and we create a **New Database** and a **New Container**

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/fcc35ed4-cd30-490c-8147-10475e4d3e38)

We first create a **New Database**. We input the **DatabaseId**: ToDoList

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/978b2f67-01c3-4711-95fd-e0eeb75a99c1)

We also create a **New Container**: Items

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/10e1d766-9c53-4ab0-9624-52f257340480)

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/95de1c25-49b8-496b-b3bb-bdeab06980ad)

## 2. Insert the new items in the Azure CosmosDB

This is the **new item** json file:

```json
{
    "id": "1",
    "partitionKey": "pk001",
    "LastName": "Smith",
    "Parents": [
        {
            "FamilyName": "Smith",
            "FirstName": "John"
        },
        {
            "FamilyName": "Smith",
            "FirstName": "Jane"
        }
    ],
    "Children": [
        {
            "FamilyName": "Smith",
            "FirstName": "Emma",
            "Gender": "Female",
            "Grade": 5,
            "Pets": [
                {
                    "GivenName": "Buddy"
                }
            ]
        },
        {
            "FamilyName": "Smith",
            "FirstName": "Mike",
            "Gender": "Male",
            "Grade": 8,
            "Pets": []
        }
    ],
    "Address": {
        "State": "California",
        "County": "Orange",
        "City": "Irvine"
    },
    "IsRegistered": true
}
```

We click in **Items** and then **New Item**

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/9a8bd5b0-7d21-4896-87e2-b8dfddd8ec54)

Then we copy and paste the json content and press **Save** button

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/775a0a33-b705-4194-8734-ea69bb11851c)

## 3. appsettings.json

We copy the **DatabaseId** and **ContainerId** 

**DatabaseId**: ToDoList

**ContainerId**: Items

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/d6a8f159-1ec9-4128-b20c-3655cebcb158)

We copy the **URI** and **Primary Key** 

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/10b9296e-6005-4a2c-9be3-9225ce19abe6)

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/ef892457-f775-4145-9260-6ce44b931dd1)

This is the **appsettings.json** file

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/b2583be5-9685-4557-9076-79f7307e94e7)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "CosmosDb": {
    "AccountEndpoint": "https://mycosmosdbluis1974.documents.azure.com:443/",
    "AccountKey": "ytnpxhMMKcs0XHkTTDhdohicyBHA1EwPFIcU8ZNSGmoH0DAXy3QaCZDW9Wxa8kzW7U3xBybEMUkUACDbiMyyyQ==",
    "DatabaseName": "ToDoList",
    "ContainerName": "Items"
  },
  "AllowedHosts": "*"
}
```

## 4. Program.cs

```csharp
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
```

## 5. Models

**Item.cs**

```csharp
using Newtonsoft.Json;

namespace AzureCosmosCRUDWebAPI.Models
{
    public class Family
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }
        public string LastName { get; set; }
        public Parent[] Parents { get; set; }
        public Child[] Children { get; set; }
        public Address Address { get; set; }
        public bool IsRegistered { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Parent
    {
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
    }

    public class Child
    {
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public int Grade { get; set; }
        public Pet[] Pets { get; set; }
    }

    public class Pet
    {
        public string GivenName { get; set; }
    }

    public class Address
    {
        public string State { get; set; }
        public string County { get; set; }
        public string City { get; set; }
    }

}
```

## 6. CosmosDbService.cs

Pay attention we set the **PartitionKey** "/Id"

```csharp
using Microsoft.Azure.Cosmos;
using AzureCosmosCRUDWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace AzureCosmosCRUDWebAPI.Services
{
    public class CosmosDbService
    {
        private Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Family item)
        {
            try
            {
                await this._container.CreateItemAsync(item, new PartitionKey(item.PartitionKey));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error in AddItemAsync. Status code: {ex.StatusCode}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddItemAsync: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        // Read an item by id
        public async Task<Family> GetItemAsync(string id, string partitionKeyValue)
        {
            try
            {
                ItemResponse<Family> response = await this._container.ReadItemAsync<Family>(id, new PartitionKey(partitionKeyValue));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        // Update an existing item
        public async Task UpdateItemAsync(string id, Family item)
        {
            await this._container.UpsertItemAsync(item, new PartitionKey(id));
        }

        // Delete an item
        public async Task DeleteItemAsync(string id, string partitionKeyValue)
        {
            await this._container.DeleteItemAsync<Family>(id, new PartitionKey(partitionKeyValue));
        }

        // List all items
        public async Task<IEnumerable<Family>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Family>(new QueryDefinition(queryString));
            List<Family> results = new List<Family>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
    }
}
```

## 7. Controller

**ItemsController.cs**

```csharp
using Microsoft.AspNetCore.Mvc;
using AzureCosmosCRUDWebAPI.Models;
using AzureCosmosCRUDWebAPI.Services;
using System.Threading.Tasks;

namespace AzureCosmosCRUDWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly CosmosDbService _cosmosDbService;

        public FamilyController(CosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        // POST api/family
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Family family)
        {
            if (family == null)
            {
                return BadRequest("Family cannot be null");
            }

            await _cosmosDbService.AddItemAsync(family);
            return CreatedAtAction(nameof(Get), new { id = family.Id }, family);
        }

        // GET api/family/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, string partitionKeyValue)
        {
            var family = await _cosmosDbService.GetItemAsync(id, partitionKeyValue);
            if (family == null)
            {
                return NotFound();
            }

            return Ok(family);
        }

        // PUT api/family/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Family family)
        {
            if (family == null || family.Id != id)
            {
                return BadRequest();
            }

            await _cosmosDbService.UpdateItemAsync(id, family);
            return NoContent();
        }

        // DELETE api/family/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, string partitionKeyValue)
        {
            await _cosmosDbService.DeleteItemAsync(id, partitionKeyValue);
            return NoContent();
        }

        // GET api/family
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var families = await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
            return Ok(families);
        }
    }
}
```

## 8. Application verification

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/88a464da-356b-4e4d-b87a-4fc9acd50ae0)

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/e4b9029e-9e68-4fe9-bc8d-7bbddb2c817c)

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/8242696a-9065-4ab7-8e45-83aa4e833289)


