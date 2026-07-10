using System.Collections.Generic;

namespace EasySQL.Test
{
    public abstract class TestBase
    {
        private static string connectString = "Host=127.0.0.1;Username=xunxin;Password=123456;Database={0}";

        public TestBase()
        {
            if (!EasySQLContext.Default.IsInitialized)
            {
                var options = new EasySQLOptions();
                options.AddDatabase(new PostgreSQLDbProxy().Config("mytest", string.Format(connectString, "mytest")));
                EasySQLContext.Default.Configure(options);
            }
        }
    }
}
