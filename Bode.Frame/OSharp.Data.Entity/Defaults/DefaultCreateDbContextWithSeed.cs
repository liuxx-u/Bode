using System.Collections.Generic;
using System.Reflection;
using OSharp.Data.Entity.Migrations;

namespace OSharp.Data.Entity.Default
{
    /// <summary>
    /// 默认数据库初始化种子
    /// </summary>
    public sealed class DefaultCreateDbContextWithSeed : CreateDatabaseIfNotExistsWithSeedBase<DefaultDbContext>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapperAssemblies"></param>
        public DefaultCreateDbContextWithSeed(ICollection<Assembly> mapperAssemblies)
        {
            MapperAssemblies = mapperAssemblies;
        }
    }
}
