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
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context; 

        public ProductosController(AppDbContext context) 
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Products.Include(p => p.Categoria).ToListAsync();
        }


        
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Products.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        
        [HttpPost]
        public async Task<ActionResult<Producto>> CreateProducto(Producto producto)
        {
            _context.Products.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Products.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Products.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }
    }
}
