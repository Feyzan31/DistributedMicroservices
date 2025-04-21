using Cassandra.Mapping.Attributes;

namespace OrderService.Models
{
    [Table("orders")]
    public class Order
    {
        [PartitionKey]
        public Guid Id { get; set; }

        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("ordered_at")]
        public DateTime OrderedAt { get; set; }
    }
}
