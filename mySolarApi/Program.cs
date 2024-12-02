// 1. Stavi sve using direktive na početak datoteke
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using SolarApp.Data; 
using SolarApp.Services;
using SolarApp.Repositories;
using SolarApp.Models;

var builder = WebApplication.CreateBuilder(args);
var openWeatherApiKey = builder.Configuration["OpenWeatherMap:ApiKey"];
var openWeatherBaseUrl = builder.Configuration["OpenWeatherMap:BaseUrl"];

builder.Services.AddDbContext<SolarDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddControllers(); 
        builder.Services.AddScoped<IProductionDataRepository, ProductionDataRepository>();

builder.Services.AddHttpClient< WeatherService>();


var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new ArgumentNullException("Jwt:Key is not configured properly in appsettings.json.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))  // Koristi provjereni jwtKey
    };
});

builder.Services.AddAuthorization(); 
builder.Services.AddHostedService<WeatherUpdateBackgroundService>();

builder.Services.AddOpenApi();

builder.Services.AddHttpClient();  
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<WeatherService>();
builder.Services.AddScoped<ProductionService>();  
builder.Services.AddScoped<ISolarPlantRepository<SolarPowerPlant>, SolarPowerPlantRepository>();
builder.Services.AddScoped<IProductionDataRepository, ProductionDataRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();

}

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();


app.Run();
