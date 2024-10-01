using System.ComponentModel;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Azure.Core.Serialization;
using AzureFunctionTest;
using AzureFunctionTest.InputConverters;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        var conString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";


        builder.Services.Configure<WorkerOptions>(options =>
        {
            // var o = new JsonSerializerOptions();
            // var sss = JsonObjectSerializer.Default;
            // o.Converters.Add(new SkipTimestampConverter());
            // // options.InputConverters.Register<MyFunctionContextInputConverter>();
            // // options.InputConverters.Register<MyJsonPocoInputConverter>();
            // options.Serializer = new JsonObjectSerializer(o);
            // options.Serializer.

            // options.Serializer = new NewtonsoftJsonObjectSerializer(new JsonSerializerSettings
            // {
            //     ContractResolver = new DefaultContractResolver
            //     {
            //         NamingStrategy = new CamelCaseNamingStrategy(),
            //     },
            //     NullValueHandling = NullValueHandling.Ignore,
            //     DateParseHandling = DateParseHandling.None,
            // });

            options.InputConverters.Clear();
            options.InputConverters.Register<MyFunctionContextInputConverter>();
            options.InputConverters.Register<MyTypeInputConverter>();
            options.InputConverters.Register<MyGuidInputConverter>();
            options.InputConverters.Register<MyDateTimeInputConverter>();
            options.InputConverters.Register<MyMemoryInputConverter>();
            options.InputConverters.Register<MyStringToByteInputConverter>();
            options.InputConverters.Register<MyJsonPocoInputConverter>();
            options.InputConverters.Register<MyArrayInputConverter>();
            options.InputConverters.Register<MyCancellationTokenInputConverter>();

            // var serializerOptions = new JsonSerializerOptions
            // {
            //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // };
            //
            // serializerOptions.Converters.Insert(0, new SkipTimestampConverter());
            //
            // options.Serializer = new JsonObjectSerializer(serializerOptions);
        });
        builder.Services.AddSingleton(sp =>
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            
            options.Converters.Insert(0, new SkipTimestampConverter());
            
            return new CosmosClientBuilder(conString)
                .WithCustomSerializer(new SystemTextJsonSerializer(options))
                .Build();
        });
    })
    // .ConfigureServices(sc =>
    // {
    //     sc.Configure<JsonSerializerOptions>(options =>
    //     {
    //         options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    //         options.Converters.Clear();
    //     });
    // })

    .Build();

host.Run();