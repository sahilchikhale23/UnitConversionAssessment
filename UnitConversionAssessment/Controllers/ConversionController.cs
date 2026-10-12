using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Data;
using UnitConversionApi.Models;

namespace UnitConversionApi.Controllers;

[ApiController]
[Route("api/v1/units")]
public class UnitController : ControllerBase
{
    // GET: api/v1/units
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(UnitRegistry.Units.Values.ToList());
    }

    // GET: api/v1/units/meter
    [HttpGet("{code}")]
    public IActionResult GetByCode(string code)
    {
        if (!UnitRegistry.Units.TryGetValue(code, out var unit))
        {
            return NotFound(new { message = "Unit not found." });
        }

        return Ok(unit);
    }

    // POST: api/v1/units
    [HttpPost]
    public IActionResult Add(AddUnitRequest request)
    {
        var validationError = ValidateUnit(request);

        if (validationError != null)
        {
            return BadRequest(new { message = validationError });
        }

        var categoryUnits = UnitRegistry.Units.Values
            .Where(u => u.Category.Equals(
                request.Category.Trim(),
                StringComparison.OrdinalIgnoreCase))
            .ToList();

        // A new category must start with its base unit.
        if (categoryUnits.Count == 0 &&
            (request.FactorToBase != 1 || request.OffsetToBase != 0))
        {
            return BadRequest(new
            {
                message = "A new category must start with " + "FactorToBase = 1 and OffsetToBase = 0."
            });
        }

        var category = categoryUnits.Count > 0 ? categoryUnits[0].Category : request.Category.Trim();

        var unit = new UnitDefinition
        {
            Code = request.Code.Trim(),
            Name = request.Name.Trim(),
            Category = category,
            FactorToBase = request.FactorToBase,
            OffsetToBase = request.OffsetToBase
        };

        if (!UnitRegistry.Units.TryAdd(unit.Code, unit))
        {
            return Conflict(new { message = "Unit code already exists." });
        }

        return CreatedAtAction(nameof(GetByCode), new { code = unit.Code }, unit);
    }

    // PUT: api/v1/units/meter
    [HttpPut("{code}")]
    public IActionResult Update(string code, AddUnitRequest request)
    {
        if (!UnitRegistry.Units.TryGetValue(code, out var existingUnit))
        {
            return NotFound(new { message = "Unit not found." });
        }

        if (!string.Equals(
                code, request.Code.Trim(),
                StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new
            {
                message = "The unit code in the URL must match the request body."
            });
        }

        var validationError = ValidateUnit(request);

        if (validationError != null)
        {
            return BadRequest(new { message = validationError });
        }

        // Keep the unit in its existing category.
        if (!existingUnit.Category.Equals(request.Category.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new
            {
                message = "Changing a unit's category is not allowed."
            });
        }

        var updatedUnit = new UnitDefinition
        {
            Code = existingUnit.Code,
            Name = request.Name.Trim(),
            Category = existingUnit.Category,
            FactorToBase = request.FactorToBase,
            OffsetToBase = request.OffsetToBase
        };

        if (!UnitRegistry.Units.TryUpdate(existingUnit.Code, updatedUnit, existingUnit))
        {
            return Conflict(new
            {
                message = "The unit changed during the update. Please retry."
            });
        }

        return Ok(updatedUnit);
    }

    // DELETE: api/v1/units/meter
    [HttpDelete("{code}")]
    public IActionResult Delete(string code)
    {
        if (!UnitRegistry.Units.TryRemove(code, out _))
        {
            return NotFound(new { message = "Unit not found." });
        }

        return Ok(new { message = $"Unit '{code}' deleted successfully." });
    }

    private static string? ValidateUnit(AddUnitRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Code) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Category))
        {
            return "Code, name, and category are required.";
        }

        if (request.FactorToBase <= 0)
        {
            return "FactorToBase must be greater than zero.";
        }

        return null;
    }
}