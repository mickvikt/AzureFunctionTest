using Microsoft.Azure.Functions.Worker.Converters;

namespace AzureFunctionTest.InputConverters;

internal sealed class MyDateTimeInputConverter : IInputConverter
{
    public ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
    {
        if (!IsValidTargetType(context) || context.Source is not string source)
        {
            return new ValueTask<ConversionResult>(ConversionResult.Unhandled());
        }

        if ((context.TargetType == typeof(DateTimeOffset) || context.TargetType == typeof(DateTimeOffset?))
            && DateTimeOffset.TryParse(source, out var parsedDateTimeOffset))
        {
            return new ValueTask<ConversionResult>(ConversionResult.Success(parsedDateTimeOffset));
        }

        if (DateTime.TryParse(source, out DateTime parsedDate))
        {
            return new ValueTask<ConversionResult>(ConversionResult.Success(parsedDate));
        }

        return new ValueTask<ConversionResult>(ConversionResult.Unhandled());
    }

    private static bool IsValidTargetType(ConverterContext context)
    {
        return context.TargetType == typeof(DateTime)
               || context.TargetType == typeof(DateTime?)
               || context.TargetType == typeof(DateTimeOffset)
               || context.TargetType == typeof(DateTimeOffset?);
    }
}