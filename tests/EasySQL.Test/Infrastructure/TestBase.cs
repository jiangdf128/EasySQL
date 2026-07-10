using Xunit;

namespace EasySQL.Test
{
    [CollectionDefinition("Database", DisableParallelization = true)]
    public class DatabaseCollection { }

    public abstract class TestBase
    {
        private static readonly object _initLock = new();
        private static bool _initialized = false;

        static TestBase()
        {
            EnsureInitialized();
        }

        private static void EnsureInitialized()
        {
            if (_initialized) return;
            lock (_initLock)
            {
                if (_initialized) return;
                var options = new EasySQLOptions();
                options.AddDatabase(new SqlServerTestProxy().Config(
                    "easysql",
                    "Server=.;Database=EasySqlTest;Trusted_Connection=True;TrustServerCertificate=True"));
                EasySQLContext.Default.Configure(options);
                _initialized = true;
            }
        }
    }
}
