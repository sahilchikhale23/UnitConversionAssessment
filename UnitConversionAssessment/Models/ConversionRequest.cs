using System.ComponentModel.DataAnnotations;

namespace UnitConversionApi.Models;

public sealed class ConversionRequest
{
    [Range(typeof(decimal), "-79228162514264337593543950335",
        "79228162514264337593543950335")]
    public decimal Value { get; set; }

    [Required]
    public string FromUnit { get; set; } = string.Empty;

    [Required]
    public string ToUnit { get; set; } = string.Empty;
}