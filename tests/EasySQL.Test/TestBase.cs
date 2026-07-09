using System.Collections.Generic;

namespace EasySQL.Test
{
    public abstract class TestBase
    {
        private static string connectString = "Host=127.0.0.1;Username=xunxin;Password=123456;Database={0}";

        public TestBase()
        {
            if (!DbContext.IsInitialized)
            {
                DbContext.ConfigContext(new List<IDbProxy>() { (new PostgreSQLDbProxy()).Config("mytest", string.Format(connectString, "mytest")) });
            }
        }
    }
}
