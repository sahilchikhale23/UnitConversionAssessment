using UnitConversionApi.Data;
using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

public sealed class ConversionService : IConversionService
{
    public ConversionResponse Convert(ConversionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.FromUnit))
        {
            throw new ArgumentException("Source unit is required.");
        }

        if (string.IsNullOrWhiteSpace(request.ToUnit))
        {
            throw new ArgumentException("Target unit is required.");
        }

        if (!UnitRegistry.Units.TryGetValue(request.FromUnit, out var sourceUnit))
        {
            throw new ArgumentException($"Unsupported source unit: {request.FromUnit}");
        }

        if (!UnitRegistry.Units.TryGetValue(request.ToUnit, out var targetUnit))
        {
            throw new ArgumentException($"Unsupported target unit: {request.ToUnit}");
        }

        if (!string.Equals(sourceUnit.Category, targetUnit.Category, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Cannot convert between different unit categories.");
        }

        ValidatePhysicalValue(request.Value, sourceUnit);

        decimal baseValue;
        decimal convertedValue;

        try
        {
            // Convert the input to its category's base unit.
            baseValue = request.Value * sourceUnit.FactorToBase + sourceUnit.OffsetToBase;

            // Convert from the base unit to the target unit.
            convertedValue = (baseValue - targetUnit.OffsetToBase) / targetUnit.FactorToBase;

            ValidatePhysicalValue(convertedValue, targetUnit);
        }
        catch (OverflowException)
        {
            throw new ArgumentException("The value is outside the supported conversion range.");
        }

        return new ConversionResponse
        {
            OriginalValue = request.Value,
            FromUnit = sourceUnit.Code,
            ToUnit = targetUnit.Code,
            ConvertedValue = convertedValue,
            Category = sourceUnit.Category
        };
    }

    private static void ValidatePhysicalValue(decimal value, UnitDefinition unit)
    {
        if (!string.Equals(unit.Category, "Temperature", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        decimal minimumValue = unit.Code.ToLowerInvariant() switch
        {
            "kelvin" => 0m,
            "celsius" => -273.15m,
            "fahrenheit" => -459.67m,
            _ => decimal.MinValue
        };

        if (value < minimumValue)
        {
            throw new ArgumentException($"{unit.Name} cannot be below absolute zero.");
        }
    }
}