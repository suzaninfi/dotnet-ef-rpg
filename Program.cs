global using dotnet_ef_rpg.Models;
global using dotnet_ef_rpg.Services.CharacterService;
global using dotnet_ef_rpg.Dtos.Character;
global using Microsoft.EntityFrameworkCore;
global using AutoMapper;
global using dotnet_ef_rpg.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
// AddScoped creates a new instance of the request service for every request
builder.Services
    .AddScoped<ICharacterService,
        CharacterService>(); // dependency injection: use CharacterService class whenever a controller wants to inject an ICharacterService
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();