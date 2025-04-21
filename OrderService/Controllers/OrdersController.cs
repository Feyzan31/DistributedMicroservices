using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository _repo;

        public OrdersController(OrderRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _repo.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order o)
        {
            if (o.Id == Guid.Empty)
                o.Id = Guid.NewGuid();

            o.OrderedAt = DateTime.UtcNow;

            await _repo.AddAsync(o);
            return CreatedAtAction(nameof(Get), new { id = o.Id }, o);
        }
    }
}
