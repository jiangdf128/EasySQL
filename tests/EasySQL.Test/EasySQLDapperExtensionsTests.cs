using Dapper;
using EasySQL;
using Xunit;

namespace EasySQL.Test
{
    /// <summary>
    /// EasySQL.Dapper 桥接扩展的单元测试。
    /// </summary>
    public class EasySQLDapperExtensionsTests
    {
        [Fact]
        public void ToDynamicParameters_ShouldConvertAllParameters()
        {
            var parameters = new Dictionary<string, object>
            {
                ["UserId"] = 123,
                ["Name"] = "test",
                ["Status"] = 1,
                ["IsActive"] = true
            };

            var dp = parameters.ToDynamicParameters();

            // DynamicParameters 没有直接的索引器，通过 ParameterNames 验证
            Assert.NotNull(dp);
            Assert.Contains("UserId", dp.ParameterNames);
            Assert.Contains("Name", dp.ParameterNames);
            Assert.Contains("Status", dp.ParameterNames);
            Assert.Contains("IsActive", dp.ParameterNames);
        }

        [Fact]
        public void ToDynamicParameters_EmptyDictionary_ShouldReturnEmptyParameters()
        {
            var parameters = new Dictionary<string, object>();
            var dp = parameters.ToDynamicParameters();
            Assert.NotNull(dp);
            Assert.Empty(dp.ParameterNames);
        }

        [Fact]
        public void ToDynamicParameters_NullDictionary_ShouldNotThrow()
        {
            Dictionary<string, object>? parameters = null;
            var dp = parameters!.ToDynamicParameters();
            Assert.NotNull(dp);
            Assert.Empty(dp.ParameterNames);
        }
    }
}
