using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product) 
        {
            await productRepository.CreateProduct(product);
            return Ok();
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
            => Ok(await productRepository.GetProducts());


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await productRepository.GetProduct(id);
            if (product is null) return NotFound();

            return product;
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product, string id)
        {
            var dbProduct = await productRepository.GetProduct(id);
            if (dbProduct is null) return BadRequest();

            product.Id = id;
            await productRepository.UpdateProduct(product);
            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleProduct(string id)
        {
            var dbProduct = await productRepository.GetProduct(id);
            if (dbProduct is null) return BadRequest();

            await productRepository.DeleteProduct(id);
            return Ok();
        }


    }
}