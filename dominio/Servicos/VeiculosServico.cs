using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.DTOs;
using MinimalApi.Infrastutura.Db;

namespace MinimalApi.Dominio.Servicos;

public class VeiculosServico : IVeiculosServico
{
    private readonly DbContexto _contexto;
    public VeiculosServico(DbContexto contexto)
    {
      _contexto = contexto;  
    }

    public void Apagar(Veiculo veiculo)
    {
        _contexto.Veiculos.Remove(veiculo);
        _contexto.SaveChanges();
    }

    public void Atualizar(Veiculo veiculo)
    {
       _contexto.Veiculos.Update(veiculo);
       _contexto.SaveChanges();
    }

    public Veiculo? BuscaPorId(int id)
    {
       return  _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
    }

    public void Incluir(Veiculo veiculo)
    {
       _contexto.Veiculos.Add(veiculo);
       _contexto.SaveChanges();
    }

    public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
    {
      var query =  _contexto.Veiculos;
      if(!string.IsNullOrEmpty(nome))
      {
         query.Where(v => v.Nome.ToLower().Contains(nome));
      }
      int itensPorPag = 10;
      if(pagina != null)
         query.Skip(((int)pagina-1)*itensPorPag).Take(itensPorPag);
      return query.ToList();
    }
}