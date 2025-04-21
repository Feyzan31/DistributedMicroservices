using Cassandra;
using ProductService.Models;

namespace ProductService.Data
{
    public class ProductRepository
    {
        private readonly Cassandra.ISession _session;

        public ProductRepository(Cassandra.ISession session)
        {
            _session = session;

            // Crée la table si elle n'existe pas (dev uniquement)
            _session.Execute(@"
                CREATE TABLE IF NOT EXISTS products (
                    id UUID PRIMARY KEY,
                    name TEXT,
                    price DECIMAL
                );
            ");
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var result = await _session.ExecuteAsync(new SimpleStatement("SELECT * FROM products"));
            var products = new List<Product>();

            foreach (var row in result)
            {
                products.Add(new Product
                {
                    Id = row.GetValue<Guid>("id"),
                    Name = row.GetValue<string>("name"),
                    Price = row.GetValue<decimal>("price")
                });
            }

            return products;
        }

        public Task AddAsync(Product p)
        {
            var stmt = new SimpleStatement(
                "INSERT INTO products (id, name, price) VALUES (?, ?, ?)",
                p.Id, p.Name, p.Price);

            return _session.ExecuteAsync(stmt);
        }
    }
}
