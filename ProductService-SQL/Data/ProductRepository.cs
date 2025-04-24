using Microsoft.EntityFrameworkCore;
using ProductService_SQL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService_SQL.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _ctx;
        public ProductRepository(ProductDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _ctx.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _ctx.Products
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> AddAsync(Product p)
        {
            _ctx.Products.Add(p);
            await _ctx.SaveChangesAsync();
            return p;
        }
    }
}
