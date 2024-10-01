using Microsoft.Azure.Functions.Worker.Converters;

namespace AzureFunctionTest.InputConverters;

public class MyGuidInputConverter : IInputConverter
{
    public ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
    {
        if (context.TargetType == typeof(Guid) || context.TargetType == typeof(Guid?))
        {
            if (context.Source is string sourceString && Guid.TryParse(sourceString, out Guid parsedGuid))
            {
                return new ValueTask<ConversionResult>(ConversionResult.Success(parsedGuid));
            }
        }

        return new ValueTask<ConversionResult>(ConversionResult.Unhandled());
    }
}