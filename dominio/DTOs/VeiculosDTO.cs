
namespace MinimalApi.DTOs;
    
public record VeiculosDTO
{
  public String Nome { get; set; }  = default!;
  public String Marca { get; set; }  = default!;
  public int Ano { get; set; }  = default!;
}

