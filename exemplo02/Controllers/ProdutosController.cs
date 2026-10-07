using exemplo02.Data;
using exemplo02.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController(AppDbContext context) : ControllerBase
    {
        // GET: api/Produtos
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await context.Produtos.ToListAsync();
        }

        // GET: api/Produtos/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // POST: api/Produtos
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetProduto", new { id = produto.ProdutoID }, produto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.ProdutoID)
            {
                return BadRequest();
            }

            var produtoExistente = await context.Produtos.FindAsync(id);

            if (produtoExistente == null)
            {
                return NotFound();
            }

            produtoExistente.CategoriaID = produto.CategoriaID;
            produtoExistente.Nome = produto.Nome;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.Preco = produto.Preco;
            produtoExistente.Estoque = produto.Estoque;
            produtoExistente.Ativo = produto.Ativo;

            await context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            context.Produtos.Remove(produto);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
