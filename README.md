# How to create a .NET8 WebAPI CRUD Azure CosmosDB for MongoDB Microservice

The code for this example is available in this github repo: https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB

## 0. Prerequisites

- Install Docker Desktop

- Install Visual Studio 2022 Community Edition version 17.8

- Install Studio 3T Free for MongoDB

## 1. Create Azure CosmosDB

We navigate to **Azure CosmosDB** service and we click in this option to create a new account:

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/0889254a-61ca-4de8-89d3-fb0627b6e2ac)

We click on **Azure CosmosDB account** button

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/d66825ed-5554-4191-a951-47b2d15cb45a)

Now we select the option **Azure Cosmos DB for NoSQL**, and we press the **create** button

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/c406a682-0658-431a-85f2-4d0726c675ce)

In following screen we input the required data for creating the service

We create a new **ResourceGroup name**: myRG

We set the **account name**: myazuremongodbaccount

We choose the service **location**: France Central

Capacity mode: **serverless**

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/bb41888b-b8c6-4c23-b413-97bb364c13a7)

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/83f49417-4496-419c-a871-48a576605b1a)

We navigate to the **Data Explorer** page and we create a **New Database** and a **New Container**

We first create a **New Database**

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/15a47c2e-441a-40b2-bc7f-ba8d97b93fc4)

We input the DatabaseId

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/95fbfa1d-dcb3-4d12-b033-844c7870da8e)

We also create a **New Container**

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/c532c95f-a5e0-4f48-9c1f-0ff8a7ddbcb3)

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/c84bf57b-a0e3-4965-9334-80a90fcac359)

## 2. Insert the new items in the Azure CosmosDB

This is the new item json file:

```json
{
  "Id": "5e9f8f8f8f8f8f8f8f8f8f8",
  "Name": "The Adventures of Sherlock Holmes",
  "Price": 15.99,
  "Category": "Mystery",
  "Author": "Arthur Conan Doyle",
  "LastName": "Doyle",
  "Parents": [
    {
      "FamilyName": "Doyle",
      "FirstName": "Charles"
    },
    {
      "FamilyName": "Doyle",
      "FirstName": "Mary"
    }
  ],
  "Children": [
    {
      "FamilyName": "Doyle",
      "FirstName": "Mary",
      "Gender": "Female",
      "Grade": 5,
      "Pets": [
        {
          "GivenName": "Rusty"
        }
      ]
    }
  ],
  "Address": {
    "State": "London",
    "County": "Greater London",
    "City": "London"
  },
  "IsRegistered": true,
  "_rid": "01010101010101010101",
  "_self": "dbs/01010101/colls/01010101/docs/01010101/",
  "_etag": "\"00000000-0000-0000-0000-000000000000\"",
  "_attachments": "attachments/",
  "_ts": 1617184000
}
```

We click in **Items** and then **New Item**

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/47342ed5-2731-4f8a-b42d-c36f59b4952f)

Then we copy and paste the json content and press **Save** button

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/d7c7f79a-7f58-497d-ae19-263ac93f3f34)

See the new item inserted:

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/5bf349bc-ed2f-48b5-934f-3b380c8f1aa5)

## 3. Update the appsettings.json file

We copy the **DatabaseId** and **ContainerId** 

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/386ff85d-4815-4dd2-a4dd-19a639dd67ac)

We copy the **URI** and **Primary Key** 

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/83009c74-275c-4a61-813e-82499d610f8a)

This is the **appsettings.json** file

![image](https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB/assets/32194879/de4d7d78-5017-46ae-a860-3374de1c1fd3)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "BookStoreDatabase": {
    "EndpointUri": "https://myazurecosmosaccount.documents.azure.com:443/",
    "PrimaryKey": "HA42SVAzPXDqY1DVPOTG80umbTCHEXXz6x2XTRekrYpYCtgK4BbCQWf4xxx55vl9NNPPG4sk3pJcACDbKB4inA==",
    "DatabaseId": "ToDoList",
    "ContainerId": "Items"
  },
  "AllowedHosts": "*"
}
```

## 4. Program.cs

```csharp
using BookStoreApi.Models;
using BookStoreApi.Services;
using MongoDB.Driver;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Read configuration from appsettings.json
ConfigurationManager Configuration = builder.Configuration;

// Azure CosmosDB configuration
string accountName = "myazurecosmosaccount"; // Name of your Azure Cosmos DB account
string endpointUri = Configuration["BookStoreDatabase:EndpointUri"];
string primaryKey = Configuration["BookStoreDatabase:PrimaryKey"];
string databaseId = Configuration["BookStoreDatabase:DatabaseId"]; // If needed
string containerId = Configuration["BookStoreDatabase:ContainerId"]; // If needed

// Construct the MongoDB connection string
string connectionString = $"mongodb://{accountName}:{Uri.EscapeDataString(primaryKey)}@{accountName}.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";

// Initialize MongoClient
var mongoClient = new MongoClient(connectionString);

// Register MongoClient with DI container
builder.Services.AddSingleton<IMongoClient>(mongoClient);

// Add services to the container.
builder.Services.Configure<BookStoreDatabaseSettings>(
    Configuration.GetSection("BookStoreDatabase"));

// Initialize and register BooksService
var booksService = await BooksService.CreateAsync(endpointUri, primaryKey, databaseId, containerId); // Ensure CreateAsync accepts necessary parameters
builder.Services.AddSingleton(booksService);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

## 5. BookService.cs

Pay attention we set the **PartitionKey** "/Id"

```csharp
using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using Microsoft.Azure.Cosmos;

namespace BookStoreApi.Services
{
    public class BooksService
    {
        private static string EndpointUri = "";
        private static string PrimaryKey = "";
        private CosmosClient cosmosClient;
        private Database database;
        private Microsoft.Azure.Cosmos.Container container;
        private static string DatabaseId = "";
        private static string ContainerId = "";

        public BooksService()
        {
        }

        // Static factory method to create and initialize an instance.
        public static async Task<BooksService> CreateAsync(string endpointUri, string primaryKey, string databaseId, string containerId)
        {
            EndpointUri = endpointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            ContainerId = containerId;

            var service = new BooksService();
            await service.InitializeAsync();
            return service;
        }

        private async Task InitializeAsync()
        {
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
            this.container = await this.database.CreateContainerIfNotExistsAsync(ContainerId, "/Id");
        }

        // Rest of the service methods
        public async Task<Book> GetBookAsync(string id)
        {
            try
            {
                ItemResponse<Book> response = await this.container.ReadItemAsync<Book>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            var query = this.container.GetItemQueryIterator<Book>(new QueryDefinition("SELECT * FROM c"));
            List <Book> results = new List<Book>();
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

## 6. Models

**Book.cs**

```csharp
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApi.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string BookName { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;

        public string Author { get; set; } = null!;

        // Additional fields to match the new data format
        [JsonPropertyName("LastName")]
        public string LastName { get; set; } = null!;

        [JsonPropertyName("Parents")]
        public List<Parent> Parents { get; set; } = new List<Parent>();

        [JsonPropertyName("Children")]
        public List<Child> Children { get; set; } = new List<Child>();

        [JsonPropertyName("Address")]
        public Address Address { get; set; } = null!;

        [JsonPropertyName("IsRegistered")]
        public bool IsRegistered { get; set; }

        // Additional system fields
        public string _rid { get; set; } = null!;
        public string _self { get; set; } = null!;
        public string _etag { get; set; } = null!;
        public string _attachments { get; set; } = null!;
        public long _ts { get; set; }
    }

    public class Parent
    {
        public string? FamilyName { get; set; }
        public string FirstName { get; set; } = null!;
    }

    public class Child
    {
        public string? FamilyName { get; set; }
        public string FirstName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public int Grade { get; set; }
        public List<Pet> Pets { get; set; } = new List<Pet>();
    }

    public class Pet
    {
        public string GivenName { get; set; } = null!;
    }

    public class Address
    {
        public string State { get; set; } = null!;
        public string County { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
```

**BookStoreDatabaseSettings.cs**

```csharp
namespace BookStoreApi.Models
{
    public class BookStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseId { get; set; } = null!;

        public string ContainerId { get; set; } = null!;
    }
}
```

## 7. Controller

```csharp

```
