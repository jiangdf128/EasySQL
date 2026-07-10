using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("Users")]
    public class User
    {
        [Key] public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public decimal Balance { get; set; }
        public short Status { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
