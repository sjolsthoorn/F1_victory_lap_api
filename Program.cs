var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Use camelCase for JSON serialization (respects JsonPropertyName attributes)
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        // Include null values in JSON
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    });

// Add CORS support
builder.Services.AddCors();

// Add memory caching
builder.Services.AddMemoryCache();

// Add Swagger/OpenAPI support with Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "F1 API",
        Version = "v1",
        Description = "API for F1 data and predictions"
    });
    
    // Include XML comments for better documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    
    // Ensure proper schema generation with nullable support
    options.UseAllOfToExtendReferenceSchemas();
    options.SupportNonNullableReferenceTypes();
    
    // Use camelCase for schema properties
    options.DescribeAllParametersInCamelCase();
});

// Add HttpClient for external API calls
builder.Services.AddHttpClient<f1_api.Services.IF1ApiService, f1_api.Services.F1ApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Always enable Swagger JSON endpoint (needed for client generation)
app.UseSwagger(options =>
{
    options.RouteTemplate = "swagger/{documentName}/swagger.json";
});

// Only enable Swagger UI in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "F1 API v1");
        options.RoutePrefix = "swagger";
    });
}

// Enable CORS for frontend
app.UseCors(policy =>
{
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
