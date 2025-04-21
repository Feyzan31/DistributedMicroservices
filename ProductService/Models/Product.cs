using Cassandra.Mapping.Attributes;

namespace ProductService.Models
{
    [Table("products")]
    public class Product
    {
        [PartitionKey]
        public Guid Id { get; set; }

        [Column("name")]
        public required string Name { get; set; }

        [Column("price")]
        public decimal Price { get; set; }
    }
}
