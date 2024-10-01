using System.Text;
using Microsoft.Azure.Functions.Worker.Converters;

namespace AzureFunctionTest.InputConverters;

public class MyStringToByteInputConverter : IInputConverter
{
    public ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
    {
        if (!(context.TargetType.IsAssignableFrom(typeof(byte[])) &&
              context.Source is string sourceString))
        {
            return new ValueTask<ConversionResult>(ConversionResult.Unhandled());
        }

        var byteArray = Encoding.UTF8.GetBytes(sourceString);
        var conversionResult = ConversionResult.Success(byteArray);

        return new ValueTask<ConversionResult>(conversionResult);
    }
}