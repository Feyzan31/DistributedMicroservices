using Cassandra;
using OrderService.Models;

namespace OrderService.Data
{
    public class OrderRepository
    {
        private readonly Cassandra.ISession _session;

        public OrderRepository(Cassandra.ISession session)
        {
            _session = session;

            _session.Execute(@"
                CREATE TABLE IF NOT EXISTS orders (
                    id UUID PRIMARY KEY,
                    product_id UUID,
                    quantity INT,
                    ordered_at TIMESTAMP
                );
            ");
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var result = await _session.ExecuteAsync(new SimpleStatement("SELECT * FROM orders"));
            var orders = new List<Order>();

            foreach (var row in result)
            {
                orders.Add(new Order
                {
                    Id = row.GetValue<Guid>("id"),
                    ProductId = row.GetValue<Guid>("product_id"),
                    Quantity = row.GetValue<int>("quantity"),
                    OrderedAt = row.GetValue<DateTime>("ordered_at")
                });
            }

            return orders;
        }

        public Task AddAsync(Order o)
        {
            var stmt = new SimpleStatement(
                "INSERT INTO orders (id, product_id, quantity, ordered_at) VALUES (?, ?, ?, ?)",
                o.Id, o.ProductId, o.Quantity, o.OrderedAt);

            return _session.ExecuteAsync(stmt);
        }
    }
}
