using Npgsql;
using System.Data;

namespace EasySQL.Test
{
    public class PostgreSQLDbProxy : DbProxyBase
    {
        public override IDbConnection Open()
        {
            var con = new NpgsqlConnection(this.ConnectString);
            if (con.State != ConnectionState.Open) {
                con.Open();
            }
            return con;
        }
    }
}
