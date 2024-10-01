using System.Text.Json.Serialization;
using AzureFunctionTest.InputConverters;
using SystemTextJsonSamples;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace AzureFunctionTest;

//[JsonConverter(typeof(JsonConverter<Poco>))]
public record Poco
{
    public string Id { get; set; }

//    [JsonConverter(typeof(SkipTimestampConverter))]
    public string DateAsString { get; set; }
}