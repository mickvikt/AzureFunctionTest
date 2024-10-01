using System.Text.Json;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Extensions.Logging;

namespace AzureFunctionTest;

public class MyCosmosDbTrigger
{
    private readonly ILogger _logger;

    public MyCosmosDbTrigger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MyCosmosDbTrigger>();
    }

    [Function("MyCosmosDbTrigger")]
    public void Run([CosmosDBTrigger(
            databaseName: "MyDatabase",
            containerName: "MyContainer",
            Connection = "CosmosDbConnectionString",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]
        string input,
        FunctionContext context)
    {
        var a = 1;
        foreach (var document in input)
        {
            this._logger.LogInformation(document.ToString());            
        } 
    }
}