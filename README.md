# F1 API

ASP.NET Core Web API for F1 data and predictions.

## OpenAPI / Swagger

This API exposes OpenAPI 3.0 specification at:
- **Swagger JSON**: `http://localhost:5241/swagger/v1/swagger.json`
- **Swagger UI** (Development only): `http://localhost:5241/swagger`

## API Contract

This backend API exposes a RESTful API for F1 data and predictions.

### Key Features

- ✅ **Nullable/Required Annotations**: Models use `[Required]` attributes and nullable reference types
- ✅ **XML Documentation**: All models and endpoints are documented with XML comments
- ✅ **Consistent Date Formats**: Dates are serialized as ISO 8601 strings
- ✅ **Enum Support**: String enums are used for better TypeScript compatibility
- ✅ **Versioned API**: Currently on `v1`, ready for future versioning

## Running the API

```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5241`
- HTTPS: `https://localhost:7095`

## Endpoints

### Drivers

- `GET /api/drivers` - Get all drivers (with pagination)
- `GET /api/drivers/{year}` - Get drivers for a specific season
- `GET /api/drivers/driver/{driverId}` - Get a specific driver by ID

## Development

### Adding New Endpoints

1. Create models in `Models/` with proper `[Required]` attributes
2. Add XML documentation comments (`/// <summary>`)
3. Create controllers in `Controllers/`
4. Use `[ProducesResponseType]` attributes for Swagger documentation

### Best Practices

- Always use nullable reference types (`string?` for optional fields)
- Add `[Required]` attributes for non-nullable properties
- Document all public APIs with XML comments
- Use DTOs (Data Transfer Objects) separate from internal entities
- Keep API contracts stable; use versioning for breaking changes
