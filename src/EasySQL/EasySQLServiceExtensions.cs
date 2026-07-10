using Microsoft.Extensions.DependencyInjection;

namespace EasySQL
{
    /// <summary>
    /// EasySQL 依赖注入扩展方法。
    /// </summary>
    /// <example>
    /// <code>
    /// // Program.cs
    /// builder.Services.AddEasySQL(options =>
    /// {
    ///     var proxy = new SqlServerProxy().Config("main", connectionString);
    ///     options.AddDatabase(proxy);
    /// });
    /// </code>
    /// </example>
    public static class EasySQLServiceExtensions
    {
        /// <summary>
        /// 注册 EasySQL 到 DI 容器，以单例生命周期提供 <see cref="IEasySQLContext"/>。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <param name="configure">配置回调。</param>
        /// <returns>服务集合（支持链式调用）。</returns>
        public static IServiceCollection AddEasySQL(
            this IServiceCollection services,
            Action<EasySQLOptions> configure)
        {
            var options = new EasySQLOptions();
            configure(options);

            // 配置全局默认实例，DI 注入的 IEasySQLContext 即为 Default 单例
            EasySQLContext.Default.Configure(options);

            services.AddSingleton<IEasySQLContext>(EasySQLContext.Default);

            return services;
        }
    }
}
