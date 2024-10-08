using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.Servicos;
using MinimalApi.DTOs;
using MinimalApi.Infrastutura.Db;
using minimals_api.Dominio.ModelViews;

#region Builder
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("secrets.json", optional: true);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculosServico, VeiculosServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("mysql");
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

#endregion
var app = builder.Build();

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion
#region Administradores
app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    if (administradorServico.Login(loginDTO) != null)
        return Results.Ok("Login realizado com sucesso");
    else
        return Results.Unauthorized();
}).WithTags("Administradores");
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody] VeiculosDTO veiculoDTO, IVeiculosServico veiculosServico) =>
{
    var veiculo = new Veiculo{  
        Nome = veiculoDTO.Nome,
        Ano = veiculoDTO.Ano,
        Marca = veiculoDTO.Marca
    };
    veiculosServico.Incluir(veiculo);
    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");
app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.Todos(pagina);
    return Results.Ok(veiculo);
}).WithTags("Veiculos");
app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);
    if(veiculo == null)
        return Results.NotFound();
        else
    return Results.Ok(veiculo);
}).WithTags("Veiculos");
app.MapPut("/veiculos/{id}", ([FromRoute] int id,  VeiculosDTO veiculoDTO,IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);
    if(veiculo == null)
        return Results.NotFound();
    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculosServico.Atualizar(veiculo);
    return Results.Ok(veiculo);
}).WithTags("Veiculos");
app.MapDelete("/veiculos/{id}", ([FromRoute] int id,IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscaPorId(id);
    if(veiculo == null)
        return Results.NotFound();

    veiculosServico.Apagar(veiculo);
    return Results.NoContent();
}).WithTags("Veiculos");

#endregion
#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
