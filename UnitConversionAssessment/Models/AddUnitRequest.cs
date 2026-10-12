using System.ComponentModel.DataAnnotations;

namespace UnitConversionApi.Models;

public sealed class AddUnitRequest
{
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    public decimal FactorToBase { get; set; }

    public decimal OffsetToBase { get; set; }
}