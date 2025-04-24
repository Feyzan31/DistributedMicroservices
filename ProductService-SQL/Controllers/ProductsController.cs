using Microsoft.AspNetCore.Mvc;
using ProductService_SQL.Data;
using ProductService_SQL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService_SQL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _repo.GetAllAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Product>> GetById(Guid id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p is null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product p)
        {
            if (p.Id == Guid.Empty)
                p.Id = Guid.NewGuid();

            var created = await _repo.AddAsync(p);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
