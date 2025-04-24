using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService_SQL.Models;

namespace OrderService_SQL.Data
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order> AddAsync(Order o);
    }
}