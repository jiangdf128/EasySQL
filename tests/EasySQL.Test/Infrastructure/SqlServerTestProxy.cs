using System.Data;
using Microsoft.Data.SqlClient;

namespace EasySQL.Test
{
    /// <summary>
    /// SQL Server 数据库代理（集成测试用）。
    /// </summary>
    public class SqlServerTestProxy : DbProxyBase
    {
        public override IDbConnection Open()
        {
            var con = new SqlConnection(this.ConnectString);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            return con;
        }
    }
}
