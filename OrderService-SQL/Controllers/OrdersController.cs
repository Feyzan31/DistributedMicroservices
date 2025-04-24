using Microsoft.AspNetCore.Mvc;
using OrderService_SQL.Data;
using OrderService_SQL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService_SQL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _repo.GetAllAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> GetById(Guid id)
        {
            var o = await _repo.GetByIdAsync(id);
            if (o is null) return NotFound();
            return Ok(o);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order o)
        {
            if (o.Id == Guid.Empty)
                o.Id = Guid.NewGuid();

            o.OrderedAt = DateTime.UtcNow;

            var created = await _repo.AddAsync(o);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}