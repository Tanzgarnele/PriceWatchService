using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PriceWatchApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            IEnumerable<Dictionary<String, Object>> products = await productRepository.GetAllAsync();
            return this.Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Int32 id)
        {
            Dictionary<String, Object> product = await productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }
            return this.Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            return this.CreatedAtAction(nameof(GetProductById), new { id = await productRepository.AddAsync(product) }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Int32 id, Product product)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            try
            {
                await productRepository.UpdateAsync(id, product);
            }
            catch (ArgumentException)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Int32 id)
        {
            await productRepository.DeleteAsync(id);
            return this.NoContent();
        }
    }
}