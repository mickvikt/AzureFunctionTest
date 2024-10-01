using System.Text;
using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureFunctionTest.InputConverters;

public class MyJsonPocoInputConverter : IInputConverter
{
    private readonly ILogger<MyJsonPocoInputConverter> logger;
    private readonly ObjectSerializer _serializer;
    
    public MyJsonPocoInputConverter(IOptions<WorkerOptions> options, ILogger<MyJsonPocoInputConverter> logger)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }
        if (options.Value.Serializer == null)
        {
            throw new InvalidOperationException(nameof(options.Value.Serializer));
        }

        this.logger = logger;

        _serializer = options.Value.Serializer;
    }
    
    public async ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
    {
        if (context.TargetType == typeof(string))
        {
            return ConversionResult.Unhandled();
        }

        this.logger.LogInformation(context.Source.ToString());
        
        byte[]? bytes = null;

        if (context.Source is string sourceString)
        {
            bytes = Encoding.UTF8.GetBytes(sourceString);
        }
        else if (context.Source is ReadOnlyMemory<byte> sourceMemory)
        {
            bytes = sourceMemory.ToArray();
        }

        if (bytes == null)
        {
            return ConversionResult.Unhandled();
        }

        return await GetConversionResultFromDeserialization(bytes, context.TargetType);
    }
    
    private async Task<ConversionResult> GetConversionResultFromDeserialization(byte[] bytes, Type type)
    {
        Stream? stream = null;

        try
        {
            stream = new MemoryStream(bytes);

            var deserializedObject = await _serializer.DeserializeAsync(stream, type, CancellationToken.None);
            return ConversionResult.Success(deserializedObject);

        }
        catch (Exception ex)
        {
            return ConversionResult.Failed(ex);
        }
        finally
        {
            if (stream != null) 
            {
#if NET5_0_OR_GREATER

                await ((IAsyncDisposable)stream).DisposeAsync();
#else
                    ((IDisposable)stream).Dispose();
#endif
            }
        }
    }
}