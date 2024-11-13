using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using examen2doP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examen2doP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsAndSalesController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductsAndSalesController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: api/productsandSales/items?q=:query
        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<Product>>> GetItems([FromQuery] string q)
        {
            var products = await _context.Products
                .Where(p => p.Title.Contains(q) || p.Description.Contains(q))
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/productsandSales/items/{id}
        [HttpGet("items/{id}")]
        public async Task<ActionResult<Product>> GetItem(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        [HttpPost("addSale")]
        public async Task<ActionResult<bool>> AddSale([FromBody] Sale sale)
        {
            var product = await _context.Products.FindAsync(sale.ProductId);
            if (product == null || product.Stock < sale.Quantity)
            {
                return BadRequest("Producto no disponible o cantidad insuficiente.");
            }

            product.Stock -= sale.Quantity;

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return Ok(true);  
        }




        // GET: api/productsandSales/sales
        [HttpGet("sales")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            var sales = await _context.Sales
                .Include(s => s.Product) 
                .ToListAsync();

            return Ok(sales);
        }
    }
}
