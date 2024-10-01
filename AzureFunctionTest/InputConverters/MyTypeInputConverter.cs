using Microsoft.Azure.Functions.Worker.Converters;

namespace AzureFunctionTest.InputConverters;

public class MyTypeInputConverter : IInputConverter
{
    public ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
    {
        Type? sourceType = context.Source?.GetType();

        if (sourceType is not null &&
            context.TargetType.IsAssignableFrom(sourceType))
        {
            var conversionResult = ConversionResult.Success(context.Source);
            return new ValueTask<ConversionResult>(conversionResult);
        }

        return new ValueTask<ConversionResult>(ConversionResult.Unhandled());
    }
}