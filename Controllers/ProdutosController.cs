using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutoCategoria(int id)
    {
        var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);
        if (produtos is null)
        {
            _logger.LogWarning("Produto não encontrado...");
            return NotFound("Produto não encontrado...");
        }

        return Ok(produtos);
    }

    [HttpGet]
    public async Task<ActionResult<IQueryable<Produto>>> Get()
    {
        var produtos = _uof.ProdutoRepository.GetAll();
        if (produtos is null)
        {
             _logger.LogWarning("Sem produtos");
            return NotFound("Sem produtos");
        }
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _uof.ProdutoRepository.Get(c => c.CategoriaId == id);
        if (produto is null)
        {
            _logger.LogWarning("Produto não encontrado...");
            return NotFound("Produto não encontrado...");
        }
        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
        {
            _logger.LogWarning("Produto não criado...");
            return NotFound("Produto não criado...");
        }

        _uof.ProdutoRepository.Create(produto);
        _uof.Commit();

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            _logger.LogWarning($"Produto não localizado...");
            return NotFound("Produto não localizado...");
        }

        var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(produtoAtualizado);

    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _uof.ProdutoRepository.Get(c => c.ProdutoId == id);

        if (produto is null)
            return NotFound("Produto não encontrado");

        var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
        _uof.Commit();

        return Ok(produtoDeletado);

    }
}