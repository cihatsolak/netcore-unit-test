using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnitTest.Web.Models;
using UnitTest.Web.Repositories;
using UnitTest.Web.Utilities;

namespace UnitTest.Web.Controllers.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiProductsController : ControllerBase
    {
        private readonly IRepository<Product> _product;
        public ApiProductsController(IRepository<Product> product)
        {
            _product = product;
        }

        [HttpGet("{a}/{b}")]
        public IActionResult Add(int a, int b)
        {
            var helper = new Helper();
            int total = helper.Add(a, b);

            return Ok(total);
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _product.GetAllAsync();
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _product.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _product.UpdateAsync(product);

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            await _product.InsertAsync(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _product.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _product.DeleteAsync(product);

            return NoContent();
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _product.GetByIdAsync(id) != null;
        }
    }
}