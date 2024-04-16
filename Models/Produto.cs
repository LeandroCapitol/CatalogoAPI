using APICatalogo.Validation;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

public class Produto : IValidatableObject
{
    [Key]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(20, ErrorMessage = "O nome deve ter entre 5 e 20 caracteres",
        MinimumLength = 5)] // data Anotations

    //[PrimeiraLetraMaiuscula] // validacao personalizada
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "A descrição de ter no máximo {1} caracteres")] // data Anotations
    public string? Descricao { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "O preço deve estar entre {1} e {2}")]// data Anotations
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 10)]// data Anotations
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validation)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {

            var primeiraLetra = this.Nome[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult("A primeira letra do nome deve ser maiúscula",
                    new[]
                    {  nameof(this.Nome) }
                    );
            }
        }
        if (this.Estoque <= 0)
        {
            yield return new ValidationResult("O estoque deve ser maior que 0",
              new[]
              {  nameof(this.Estoque) }
              );
        }
    }
}
