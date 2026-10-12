# Unit Conversion API

A RESTful API built with **ASP.NET Core Web API and C#** that converts values between different units of measurement.

The API supports unit categories such as Length, Mass, and Temperature. It also provides endpoints to add, view, update, and delete unit definitions dynamically.

## Features

- Convert values between supported units.
- Support multiple categories, including Length, Mass, and Temperature.
- Add new units and categories.
- View all available units or retrieve a specific unit.
- Update and delete unit definitions.
- Validate unit categories and conversion factors.
- Use a common base unit and conversion formula to simplify adding new units.

## Technologies Used

- C#
- ASP.NET Core Web API
- REST APIs
- Swagger / OpenAPI
- In-memory storage using `ConcurrentDictionary`

## Getting Started

### Prerequisites

- .NET SDK compatible with the project.
- Visual Studio or Visual Studio Code.
- Git (optional).

You can use Swagger to execute and test every API endpoint.

## API Endpoints

The API provides the following endpoints:

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/v1/conversions` | Convert a value between units |
| GET | `/api/v1/units` | Get all available units |
| GET | `/api/v1/units/{code}` | Get a unit by its code |
| POST | `/api/v1/units` | Add a new unit |
| PUT | `/api/v1/units/{code}` | Update an existing unit |
| DELETE | `/api/v1/units/{code}` | Delete a unit |

---

## 1. Convert Units

**Endpoint:** `POST /api/v1/conversions`

Converts a value from one unit to another within the same category.

### Request body

```json
{
  "value": 100,
  "fromUnit": "meter",
  "toUnit": "kilometer"
}
```

### Successful response

```json
{
  "originalValue": 100,
  "fromUnit": "meter",
  "toUnit": "kilometer",
  "convertedValue": 0.1,
  "category": "Length"
}
```

### How it works

1. Provide the value you want to convert.
2. Specify the source unit in `fromUnit`.
3. Specify the destination unit in `toUnit`.
4. The API validates the units and ensures both belong to the same category.
5. The API calculates and returns the converted value.

### More examples

**Mass:** Convert grams to kilograms.

```json
{
  "value": 500,
  "fromUnit": "gram",
  "toUnit": "kilogram"
}
```

Expected converted value: `0.5`

**Temperature:** Convert Celsius to Fahrenheit.

```json
{
  "value": 25,
  "fromUnit": "celsius",
  "toUnit": "fahrenheit"
}
```

Expected converted value: approximately `77`

### Possible errors

- `400 Bad Request` — Invalid or unsupported unit, incompatible categories, or invalid temperature value.
- `400 Bad Request` — Invalid request data.

---

## 2. Get All Available Units

**Endpoint:** `GET /api/v1/units`

Returns all unit definitions currently registered in the application.

### Example request

```http
GET /api/v1/units
```

### Example response

```json
[
  {
    "code": "meter",
    "name": "Meter",
    "category": "Length",
    "factorToBase": 1,
    "offsetToBase": 0
  },
  {
    "code": "kilometer",
    "name": "Kilometer",
    "category": "Length",
    "factorToBase": 1000,
    "offsetToBase": 0
  },
  {
    "code": "kilogram",
    "name": "Kilogram",
    "category": "Mass",
    "factorToBase": 1,
    "offsetToBase": 0
  }
]
```

The actual response contains all units currently registered in the application.

### When to use it

Use this endpoint to discover which units are available before performing a conversion.

---

## 3. Get a Unit by Code

**Endpoint:** `GET /api/v1/units/{code}`

Returns the details of a specific unit.

### Example request

```http
GET /api/v1/units/meter
```

### Example response

```json
{
  "code": "meter",
  "name": "Meter",
  "category": "Length",
  "factorToBase": 1,
  "offsetToBase": 0
}
```

### Possible errors

`404 Not Found` — The requested unit does not exist.

---

## 4. Add a New Unit

**Endpoint:** `POST /api/v1/units`

Adds a new unit to the registry. You can add a unit to an existing category or create a new category by registering its base unit first.

### Request body

Example: Add a minute to the Time category.

```json
{
  "code": "minute",
  "name": "Minute",
  "category": "Time",
  "factorToBase": 60,
  "offsetToBase": 0
}
```

### Successful response

The API returns `201 Created` with the newly created unit.

```json
{
  "code": "minute",
  "name": "Minute",
  "category": "Time",
  "factorToBase": 60,
  "offsetToBase": 0
}
```

### Add another unit to the same category

Example: Add an hour.

```json
{
  "code": "hour",
  "name": "Hour",
  "category": "Time",
  "factorToBase": 3600,
  "offsetToBase": 0
}
```

Now the API can convert between minutes and hours without requiring new conversion logic.

### Understanding the fields

| Field | Meaning |
|---|---|
| `code` | Unique identifier used in conversion requests |
| `name` | Display name of the unit |
| `category` | Measurement category, such as Length, Mass, or Time |
| `factorToBase` | Multiplier used to convert the unit to its category's base unit |
| `offsetToBase` | Additional value used for offset-based conversions, especially temperature |

### Important rules

- The unit code must be unique.
- The conversion factor must be greater than zero.
- A new category must start with a base unit using `factorToBase: 1` and `offsetToBase: 0`.
- Units within the same category must use a consistent base unit.
- Temperature units require correct factors and offsets relative to Kelvin.

### Possible errors

- `400 Bad Request` — Invalid unit details or conversion factor.
- `409 Conflict` — A unit with the same code already exists.

---

## 5. Update an Existing Unit

**Endpoint:** `PUT /api/v1/units/{code}`

Updates the name, conversion factor, or offset of an existing unit.

### Example request

```http
PUT /api/v1/units/minute
```

### Request body

```json
{
  "code": "minute",
  "name": "Minute",
  "category": "Time",
  "factorToBase": 60,
  "offsetToBase": 0
}
```

### Successful response

The API returns `200 OK` with the updated unit definition.

### Important rules

- The unit must already exist.
- The code in the URL must match the code in the request body.
- The unit's category cannot be changed through this endpoint.
- The conversion factor must be greater than zero.
- If the conversion factor or offset changes, subsequent conversions use the updated definition.

### Possible errors

- `400 Bad Request` — Invalid request or mismatched unit code.
- `404 Not Found` — The unit does not exist.
- `409 Conflict` — The unit changed concurrently and the update could not be applied.

**Note:** Updating a base unit's conversion factor can affect all conversions in that category. Ensure the new factor is correct before updating a unit.

---

## 6. Delete a Unit

**Endpoint:** `DELETE /api/v1/units/{code}`

Deletes an existing unit from the registry.

### Example request

```http
DELETE /api/v1/units/minute
```

### Successful response

```json
{
  "message": "Unit 'minute' deleted successfully."
}
```

### Possible errors

`404 Not Found` — The unit does not exist.

### Important considerations

- A deleted unit can no longer be used in new conversion requests.
- Deleting a unit does not delete other units in its category.
- The current implementation does not prevent deletion of base units. Avoid deleting a base unit while other units depend on it.

---

## Understanding the Conversion Formula

The API uses a common base unit for each category.

First, convert the source value to the base unit:

```text
baseValue = value * sourceFactor + sourceOffset
```

Then convert the base value to the target unit:

```text
result = (baseValue - targetOffset) / targetFactor
```

For example, the base unit for Length is Meter:

- 1 kilometer = 1,000 meters.
- 1 centimeter = 0.01 meters.

The same formula works for many categories, so adding a new unit usually requires only its definition, not a new conversion method.

For temperature, Kelvin is the base unit, and offsets are required because Celsius and Fahrenheit have different zero points.

## Error Handling

The API uses standard HTTP status codes.

| Status code | Meaning |
|---|---|
| `200 OK` | Request completed successfully |
| `201 Created` | New unit created successfully |
| `400 Bad Request` | Invalid input or incompatible units |
| `404 Not Found` | Requested unit does not exist |
| `409 Conflict` | Duplicate unit code or concurrent update conflict |

---

## Project Structure

```text
UnitConversionApi/
├── Controllers/
│   ├── ConversionController.cs
│   └── UnitController.cs
├── Data/
│   └── UnitRegistry.cs
├── Models/
│   ├── ConversionRequest.cs
│   ├── ConversionResponse.cs
│   ├── UnitDefinition.cs
│   └── AddUnitRequest.cs
├── Services/
│   ├── IConversionService.cs
│   └── ConversionService.cs
├── Program.cs
├── appsettings.json
└── README.md
```

## Current Limitations

- **In-memory storage:** Added, updated, and deleted units are lost when the application restarts.
- **No authentication:** The write endpoints should be protected before deploying the API publicly.
- **Unit validation:** Custom temperature units must have correct factors and offsets, and absolute-zero validation should be based on the common temperature scale.
- **Base unit management:** Deleting or changing a base unit can make other units in the same category invalid.

For a production application, persistent database storage, authorization, stronger validation, and base-unit dependency checks are recommended.

## Future Improvements

- Store unit definitions in a database.
- Add authentication and authorization.
- Add pagination and filtering to the units endpoint.
- Add automated unit and integration tests.
- Support more measurement categories, such as Area, Volume, Speed, Energy, and Pressure.

## License

Add your preferred license before distributing this project publicly.
