using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService_SQL.Models;

namespace ProductService_SQL.Data
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> AddAsync(Product p);
    }
}

