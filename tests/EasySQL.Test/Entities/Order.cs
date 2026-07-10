using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("Orders")]
    public class Order
    {
        [Key] public long Id { get; set; }
        public int UserId { get; set; }
        public string OrderNo { get; set; } = "";
        public decimal TotalAmount { get; set; }
        public short Status { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
