global using dotnet_ef_rpg.Models;
global using dotnet_ef_rpg.Services.CharacterService;
global using dotnet_ef_rpg.Dtos.Character;
global using Microsoft.EntityFrameworkCore;
global using AutoMapper;
global using dotnet_ef_rpg.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Adding this adds the option to authenticate with bearer token to the Swagger UI
builder.Services.AddSwaggerGen(
    config =>
    {
        config.AddSecurityDefinition("oauth2",
            new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme. Example: 'bearer {token}'",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
        config.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
// AddScoped creates a new instance of the request service for every request
builder.Services
    .AddScoped<ICharacterService,
        CharacterService>(); // dependency injection: use CharacterService class whenever a controller wants to inject an ICharacterService
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
// Add this so that you can require authentication for using the controllers or specific controller methods
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// This line should specifically be here, above UseAuthorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();