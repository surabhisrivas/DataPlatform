using DataPlatform.API;
using DataPlatform.Application;
using DataPlatform.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var _myAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Conventions.Insert(0, new RoutePrefixConvention("api")); // To add prefix "api" in the APIs.
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

AppConfigure.AddCorsSettings(builder.Services, _myAllowSpecificOrigins);
AppConfigure.AddHttpClient(builder.Services);

var pathBase = "/";

var app = builder.Build();

app.UseCors(_myAllowSpecificOrigins);
app.UsePathBase(pathBase);

// Write logic to redirect by default swagger page when the application starts.
// And also fix trailing slash url issue.
var option = new RewriteOptions();
// Redirect root to /scalar
option.AddRedirect("^$", "/scalar", 302);
app.UseRewriter(option);

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Add Scalar UI(a modern Swagger-style UI, built into .NET)
    app.MapScalarApiReference();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
