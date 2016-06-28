
using System;
using System.Data.Entity;

namespace OSharp.Data.Entity.Default
{
    /// <summary>
    /// 默认 上下文初始化操作类
    /// </summary>
    public sealed class DefaultDbContextInitializer : DbContextInitializerBase<DefaultDbContext>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultDbContextInitializer()
        {
            //添加迁移种子类
            CreateDatabaseInitializer = new DefaultCreateDbContextWithSeed(MapperAssemblies);
            MigrateInitializer = new MigrateDatabaseToLatestVersion<DefaultDbContext, DefaultMigrationsConfigurationWithSeed>(false, new DefaultMigrationsConfigurationWithSeed(MapperAssemblies));
        }
    }
}