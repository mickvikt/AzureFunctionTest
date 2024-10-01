using Microsoft.Azure.Functions.Worker.Converters;

namespace AzureFunctionTest.InputConverters;

public class MyCancellationTokenInputConverter : IInputConverter
{
    public ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
    {
        if (context.TargetType == typeof(CancellationToken) || context.TargetType == typeof(CancellationToken?))
        {
            return new ValueTask<ConversionResult>(ConversionResult.Success(context.FunctionContext.CancellationToken));
        }

        return new ValueTask<ConversionResult>(ConversionResult.Unhandled());
    }
}