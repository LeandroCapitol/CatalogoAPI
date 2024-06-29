using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class CategoriaDTO
{
    public int CategoriaId { get; set; }

    //Essas anotações são usadas para garantir que os dados inseridos no DTO CategoriaDTO
    //atendam a certos critérios de validação, como serem obrigatórios e terem comprimentos
    //máximos especificados. Isso ajuda a garantir que os dados inseridos sejam válidos e
    //atendam aos requisitos específicos da sua aplicação.
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

}
