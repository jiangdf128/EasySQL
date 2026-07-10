using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("OrderItems")]
    public class OrderItem
    {
        [Key] public long Id { get; set; }
        public long OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
