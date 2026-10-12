using System.ComponentModel.DataAnnotations;

namespace UnitConversionApi.Models;

public sealed class ConversionRequest
{
    public decimal Value { get; set; }

    [Required]
    public string FromUnit { get; set; } = string.Empty;

    [Required]
    public string ToUnit { get; set; } = string.Empty;
}