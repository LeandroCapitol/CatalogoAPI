using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {}
        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();

            var categoriaOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

            // var resultado = IPagedList<Categoria>.ToPagedList(categoriaOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            var resultado = await categoriaOrdenadas.ToPagedListAsync(categoriasParameters.PageNumber,
                                                                categoriasParameters.PageSize);

            return resultado;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriasFiltroNome.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroNome.Nome));
            }

            //var categoriaFiltrada = IPagedList<Categoria>.ToPagedList(categorias.AsQueryable(),
            //                                        categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);

            var categoriaFiltrada = await categorias.ToPagedListAsync(categoriasFiltroNome.PageNumber,
                                                                   categoriasFiltroNome.PageSize);

            return categoriaFiltrada;
        }
    }
}
