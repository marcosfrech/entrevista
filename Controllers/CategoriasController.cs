using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApi.Models; 
using MyApi.Data; 
namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context) 
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categories.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategorias(int id)
        {
            var categoria = await _context.Categories.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        
        [HttpPost]
        public async Task<ActionResult<Categoria>> CreateCategorias(Categoria categoria)
        {
            _context.Categories.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategorias), new { id = categoria.Id }, categoria);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategorias(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categories.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            var tieneProductos = await _context.Products.AnyAsync(p => p.Categoria.Id == id);
            if (tieneProductos)
            {
                return BadRequest("No se puede eliminar la categoría porque tiene productos asociados.");
            }

            _context.Categories.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool CategoriaExist(int id)
        {
            return _context.Categories.Any(p => p.Id == id);
        }
    }
}

