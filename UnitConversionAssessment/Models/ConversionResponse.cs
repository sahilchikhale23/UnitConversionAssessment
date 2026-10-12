namespace UnitConversionApi.Models;

public sealed class ConversionResponse
{
    public decimal OriginalValue { get; set; }

    public string FromUnit { get; set; } = string.Empty;

    public string ToUnit { get; set; } = string.Empty;

    public decimal ConvertedValue { get; set; }

    public string Category { get; set; } = string.Empty;
}