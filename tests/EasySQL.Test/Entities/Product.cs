using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("Products")]
    public class Product
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public short Status { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
