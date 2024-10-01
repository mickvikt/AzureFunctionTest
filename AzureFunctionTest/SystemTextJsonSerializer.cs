using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Core.Serialization;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace AzureFunctionTest;

public class SystemTextJsonSerializer : CosmosLinqSerializer
{
    private readonly JsonObjectSerializer systemTextJsonSerializer;

    public SystemTextJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        jsonSerializerOptions.Converters.Clear();
        jsonSerializerOptions.Converters.Add(new SkipTimestampConverter());
        this.systemTextJsonSerializer = new JsonObjectSerializer(jsonSerializerOptions);
    }

    public override T FromStream<T>(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        using (stream)
        {
            if (stream.CanSeek && stream.Length == 0)
            {
                return default;
            }

            if (typeof(Stream).IsAssignableFrom(typeof(T)))
            {
                return (T)(object)stream;
            }
            
            return (T)this.systemTextJsonSerializer.Deserialize(stream, typeof(T), default);
        }
    }

    public override Stream ToStream<T>(T input)
    {
        MemoryStream streamPayload = new MemoryStream();
        this.systemTextJsonSerializer.Serialize(streamPayload, input, input.GetType(), default);
        streamPayload.Position = 0;
        return streamPayload;
    }

    public override string SerializeMemberName(MemberInfo memberInfo)
    {
        System.Text.Json.Serialization.JsonExtensionDataAttribute jsonExtensionDataAttribute =
            memberInfo.GetCustomAttribute<System.Text.Json.Serialization.JsonExtensionDataAttribute>(true);
        if (jsonExtensionDataAttribute != null)
        {
            return null;
        }

        JsonPropertyNameAttribute jsonPropertyNameAttribute = memberInfo.GetCustomAttribute<JsonPropertyNameAttribute>(true);

        string memberName = !string.IsNullOrEmpty(jsonPropertyNameAttribute?.Name)
            ? jsonPropertyNameAttribute.Name
            : memberInfo.Name;

        // Users must add handling for any additional attributes here

        return memberName;
    }
}