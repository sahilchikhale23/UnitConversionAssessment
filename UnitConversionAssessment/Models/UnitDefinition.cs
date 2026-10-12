namespace UnitConversionApi.Models;

public sealed class UnitDefinition
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public decimal FactorToBase { get; set; }

    public decimal OffsetToBase { get; set; }
}