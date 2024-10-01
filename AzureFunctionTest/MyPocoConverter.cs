using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using AzureFunctionTest;

namespace SystemTextJsonSamples
{
    public class MyPocoConverter : JsonConverter<Poco>
    {
        public override Poco Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var x = reader.GetString();
            return new Poco();
        }


        public override void Write(
            Utf8JsonWriter writer,
            Poco value,
            JsonSerializerOptions options) =>
            writer.WriteStringValue("labas");
    }
}