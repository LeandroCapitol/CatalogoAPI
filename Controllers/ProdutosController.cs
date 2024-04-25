using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IProdutoRepository repository, ILogger<ProdutosController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IQueryable<Produto>>> Get()
    {
        var produtos = _repository.GetProdutos();
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
        var produto = _repository.GetProduto(id);
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

        _repository.Create(produto);
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

        bool atuallizado = _repository.Update(produto);

        if (atuallizado)
            return Ok(produto);

        return StatusCode(500,$"falha ao atualizar o produto de id = {id}");
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        bool produto = _repository.Delete(id);

        if (produto)
            return Ok($"Produto id = {id} excluído!");

        return StatusCode(500, $"falha ao excluir o produto de id = {id}");

    }
}