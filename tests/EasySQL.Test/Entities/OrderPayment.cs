using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("OrderPayments")]
    public class OrderPayment
    {
        [Key] public long Id { get; set; }
        public long OrderId { get; set; }
        public short PayMethod { get; set; } = 1;
        public decimal Amount { get; set; }
        public DateTime? PayTime { get; set; }
        public short Status { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
