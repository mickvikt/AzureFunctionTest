using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Converters;

namespace AzureFunctionTest.InputConverters;

public class MyFunctionContextInputConverter : IInputConverter
{
    public ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
    {
        // Special handling for the context.
        if (context.TargetType == typeof(FunctionContext))
        {
            return new ValueTask<ConversionResult>(ConversionResult.Success(context.FunctionContext));
        }

        return new ValueTask<ConversionResult>(ConversionResult.Unhandled());
    } 
}