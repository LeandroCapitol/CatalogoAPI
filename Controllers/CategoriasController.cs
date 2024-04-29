using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
       var categorias = _uof.CategoriaRepository.GetAll();

        if (categorias is null)
            return NotFound("Não existem categorias...");

        var listaCategoriaDto = new List<CategoriaDTO>();

        foreach(var categoria in listaCategoriaDto)
        {
            var categoriaDto = new CategoriaDTO()
            {
                ImagemUrl = categoria.ImagemUrl,
                Nome = categoria.Nome,
                CategoriaId = categoria.CategoriaId,
            };

            listaCategoriaDto.Add(categoriaDto);
        }

       return Ok(listaCategoriaDto);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        var categoriaDto = new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl,
        };

        return Ok(categoriaDto);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = new Categoria()
        {
            CategoriaId = categoriaDto.CategoriaId,
            ImagemUrl= categoriaDto.ImagemUrl,
            Nome= categoriaDto.Nome,
        };

        var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
        _uof.Commit();

        var novaCategoriaDto = new CategoriaDTO()
        {
            CategoriaId = categoriaCriada.CategoriaId,
            ImagemUrl = categoriaCriada.ImagemUrl,
            Nome = categoriaCriada.Nome,
        };

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = new Categoria()
        {
            Nome = categoriaDto.Nome,
            CategoriaId = categoriaDto.CategoriaId,
            ImagemUrl= categoriaDto.ImagemUrl,
        };

        var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
        _uof.Commit();

        var NovaCategoriaDto = new CategoriaDTO()
        {
            Nome = categoriaAtualizada.Nome,
            CategoriaId = categoriaAtualizada.CategoriaId,
            ImagemUrl = categoriaAtualizada.ImagemUrl,
        };

        return Ok(NovaCategoriaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id={id} não encontrada...");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
        _uof.Commit();

        var NovaCategoriaDto = new CategoriaDTO()
        {
            Nome = categoriaExcluida.Nome,
            CategoriaId = categoriaExcluida.CategoriaId,
            ImagemUrl = categoriaExcluida.ImagemUrl,
        };


        return Ok(NovaCategoriaDto);
    }
}