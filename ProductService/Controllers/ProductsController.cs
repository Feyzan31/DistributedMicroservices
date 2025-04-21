using Microsoft.AspNetCore.Mvc;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _repo;

        public ProductsController(ProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _repo.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product p)
        {
            if (p.Id == Guid.Empty)
            {
                p.Id = Guid.NewGuid(); // génère un ID unique si non fourni
            }

            await _repo.AddAsync(p);
            return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
        }
    }
}
