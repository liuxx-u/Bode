using System.Collections.Generic;
using System.Reflection;
using OSharp.Data.Entity.Migrations;

namespace OSharp.Data.Entity.Default
{
    /// <summary>
    /// 默认数据库迁移配置类
    /// </summary>
    public sealed class DefaultMigrationsConfigurationWithSeed : MigrationsConfigurationWithSeedBase<DefaultDbContext>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultMigrationsConfigurationWithSeed() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapperAssemblies">种子映射程序集</param>
        public DefaultMigrationsConfigurationWithSeed(ICollection<Assembly> mapperAssemblies)
        {
            MapperAssemblies = mapperAssemblies;
        }
    }
}
