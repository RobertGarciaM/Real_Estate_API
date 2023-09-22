using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Real_Estate_API;
using RealEstate.Mediator.AutoMapperProfile;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<InMemoryDbContext>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("RealEstate.Mediator")));


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Configuraci�n para permitir la carga de archivos
    c.UseOneOfForPolymorphism(); // Si est�s utilizando tipos complejos

    // Configura el soporte para cargar archivos en Swagger
    c.OperationFilter<SwaggerFileUploadFilter>(); // Debes crear esta clase
});


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
