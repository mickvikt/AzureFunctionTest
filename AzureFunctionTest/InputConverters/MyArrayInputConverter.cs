﻿using Microsoft.Azure.Functions.Worker.Converters;

namespace AzureFunctionTest.InputConverters;

public class MyArrayInputConverter : IInputConverter
{
    public ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
    {
        object? target = null;
        // Ensure requested type is an array
        if (context.TargetType.IsArray)
        {
            Type? elementType = context.TargetType.GetElementType();
            if (elementType is not null)
            {
                // Ensure that we can assign from source to parameter type
                if (elementType.Equals(typeof(string))
                    || elementType.Equals(typeof(byte[]))
                    || elementType.Equals(typeof(ReadOnlyMemory<byte>))
                    || elementType.Equals(typeof(long))
                    || elementType.Equals(typeof(double)))
                {
                    target = context.Source switch
                    {
                        IEnumerable<string> source => source.ToArray(),
                        IEnumerable<ReadOnlyMemory<byte>> source => GetBinaryData(source, elementType!),
                        IEnumerable<double> source => source.ToArray(),
                        IEnumerable<long> source => source.ToArray(),
                        _ => null
                    };
                }
            }
        }

        if (target is not null)
        {
            return new ValueTask<ConversionResult>(ConversionResult.Success(target));
        }

        return new ValueTask<ConversionResult>(ConversionResult.Unhandled());
    }

    private static object? GetBinaryData(IEnumerable<ReadOnlyMemory<byte>> source, Type targetType)
    {
        if (targetType.IsAssignableFrom(typeof(ReadOnlyMemory<byte>)))
        {
            return source.ToArray();
        }
        else
        {
            return source.Select(i => i.ToArray()).ToArray();
        }
    }
}