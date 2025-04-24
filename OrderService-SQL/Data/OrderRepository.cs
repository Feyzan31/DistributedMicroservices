using Microsoft.EntityFrameworkCore;
using OrderService_SQL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService_SQL.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _ctx;
        public OrderRepository(OrderDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _ctx.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _ctx.Orders
                             .AsNoTracking()
                             .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> AddAsync(Order o)
        {
            _ctx.Orders.Add(o);
            await _ctx.SaveChangesAsync();
            return o;
        }
    }
}