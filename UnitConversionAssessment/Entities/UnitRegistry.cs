using System.Collections.Concurrent;
using UnitConversionApi.Models;

namespace UnitConversionApi.Data;

public static class UnitRegistry
{
    public static readonly ConcurrentDictionary<string, UnitDefinition> Units =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // Length: Base unit = Meter

            ["meter"] = new()
            {
                Code = "meter",
                Name = "Meter",
                Category = "Length",
                FactorToBase = 1m
            },

            ["kilometer"] = new()
            {
                Code = "kilometer",
                Name = "Kilometer",
                Category = "Length",
                FactorToBase = 1000m
            },

            ["centimeter"] = new()
            {
                Code = "centimeter",
                Name = "Centimeter",
                Category = "Length",
                FactorToBase = 0.01m
            },

            ["millimeter"] = new()
            {
                Code = "millimeter",
                Name = "Millimeter",
                Category = "Length",
                FactorToBase = 0.001m
            },

            ["mile"] = new()
            {
                Code = "mile",
                Name = "Mile",
                Category = "Length",
                FactorToBase = 1609.344m
            },

            ["yard"] = new()
            {
                Code = "yard",
                Name = "Yard",
                Category = "Length",
                FactorToBase = 0.9144m
            },

            ["foot"] = new()
            {
                Code = "foot",
                Name = "Foot",
                Category = "Length",
                FactorToBase = 0.3048m
            },

            ["inch"] = new()
            {
                Code = "inch",
                Name = "Inch",
                Category = "Length",
                FactorToBase = 0.0254m
            },

            // Mass: Base unit = Kilogram

            ["kilogram"] = new()
            {
                Code = "kilogram",
                Name = "Kilogram",
                Category = "Mass",
                FactorToBase = 1m
            },

            ["gram"] = new()
            {
                Code = "gram",
                Name = "Gram",
                Category = "Mass",
                FactorToBase = 0.001m
            },

            ["milligram"] = new()
            {
                Code = "milligram",
                Name = "Milligram",
                Category = "Mass",
                FactorToBase = 0.000001m
            },

            ["tonne"] = new()
            {
                Code = "tonne",
                Name = "Metric Tonne",
                Category = "Mass",
                FactorToBase = 1000m
            },

            ["pound"] = new()
            {
                Code = "pound",
                Name = "Pound",
                Category = "Mass",
                FactorToBase = 0.45359237m
            },

            ["ounce"] = new()
            {
                Code = "ounce",
                Name = "Ounce",
                Category = "Mass",
                FactorToBase = 0.028349523125m
            },

            // Temperature: Base unit = Kelvin

            ["kelvin"] = new()
            {
                Code = "kelvin",
                Name = "Kelvin",
                Category = "Temperature",
                FactorToBase = 1m,
                OffsetToBase = 0m
            },

            ["celsius"] = new()
            {
                Code = "celsius",
                Name = "Celsius",
                Category = "Temperature",
                FactorToBase = 1m,
                OffsetToBase = 273.15m
            },

            ["fahrenheit"] = new()
            {
                Code = "fahrenheit",
                Name = "Fahrenheit",
                Category = "Temperature",
                FactorToBase = 5m / 9m,
                OffsetToBase = 459.67m * 5m / 9m
            }
        };
}