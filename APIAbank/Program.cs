using Core.APIAbank.Interfaces;
using Infrastructure.APIAbank.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System;
using Core.APIAbank.Repository;
using APIAbank;
using Microsoft.AspNetCore.Mvc;
using APIAbank.Controllers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<JwtService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbConnection>(db => new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=pruebaAbankBD;Integrated Security=True;Encrypt=False"));
var app = builder.Build();


// Crear un endpoint para obtener el JWT
app.MapPost("/generate-token", (JwtService jwtService, [FromBody] Usuariotoken usuario) =>
{
    // Generar token con teléfono y contraseña
    var token = jwtService.GenerateToken(usuario.Telefono, usuario.Contrasena);
    return Results.Ok(new { token });
});

app.MapGet("/validate-token", (JwtService jwtService, [FromHeader(Name = "Authorization")] string authorization) =>
{
    // Extraer el token del header
    var token = authorization?.Replace("Bearer ", "");

    if (string.IsNullOrEmpty(token))
    {
        return Results.BadRequest("Token no proporcionado");
    }

    var principal = jwtService.ValidateToken(token);
    if (principal == null)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new { mensaje = "Token válido" });
});

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
public class Usuariotoken
{
    public string Telefono { get; set; }
    public string Contrasena { get; set; }
}