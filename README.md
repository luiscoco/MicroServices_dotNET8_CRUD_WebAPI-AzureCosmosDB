# How to create a .NET8 WebAPI CRUD Azure CosmosDB for MongoDB Microservice

The code for this example is available in this github repo: https://github.com/luiscoco/MicroServices_dotNET8_CRUD_WebAPI-AzureCosmosDB

## 0. Prerequisites

- Install Docker Desktop

- Install Visual Studio 2022 Community Edition version 17.8

- Install Studio 3T Free for MongoDB

## 1. Create Azure CosmosDB

We navigate to **Azure CosmosDB** service and we click in this option to create a new account:


We click on **Azure CosmosDB account** button



Now we select the option **Azure Cosmos DB for NoSQL**, and we press the **create** button



In following screen we input the required data for creating the service

We create a new **ResourceGroup name**: myRG

We set the **account name**: myazuremongodbaccount

We choose the service **location**: France Central

Capacity mode: **serverless**



We navigate to the **Data Explorer** page and we create a **New Database** and a **New Container**

We first create a **New Database**



We input the DatabaseId



We also create a **New Container**



## 2. Insert the new items in the Azure CosmosDB

This is the new item json file:

```json

```

We click in **Items** and then **New Item**



Then we copy and paste the json content and press **Save** button



See the new item inserted:



## 3. Update the appsettings.json file

We copy the **DatabaseId** and **ContainerId** 



We copy the **URI** and **Primary Key** 



This is the **appsettings.json** file



```json

```

## 4. Program.cs

```csharp

```

## 5. BookService.cs

Pay attention we set the **PartitionKey** "/Id"

```csharp

```

## 6. Models

**Book.cs**

```csharp

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
