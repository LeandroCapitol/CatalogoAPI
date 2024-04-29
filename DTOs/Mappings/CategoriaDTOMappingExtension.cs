using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings
{
    public static class CategoriaDTOMappingExtension
    {
        public static CategoriaDTO? ToCategoriaDTO(this CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null)
                return null;

            return new CategoriaDTO
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl,
            };
        }

        public static Categoria? ToCategoria(this CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null)
                return null;

            return new Categoria
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl,
            };
        }

        public static IEnumerable<CategoriaDTO?> ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
        {
            if (!categorias.Any() || categorias is null) return new List<CategoriaDTO>();

            return categorias.Select(categorias => new CategoriaDTO
            {
                CategoriaId = categorias.CategoriaId,
                Nome = categorias.Nome,
                ImagemUrl = categorias.ImagemUrl,
            }).ToList();
        }
    }
}
