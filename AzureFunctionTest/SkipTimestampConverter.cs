using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureFunctionTest;

public class SkipTimestampConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Return the string value without parsing it as a DateTime
        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(JsonSerializer.Serialize(value));
    }    
}