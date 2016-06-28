using OSharp.Data.Entity.Migrations;

namespace OSharp.Data.Entity.Default
{
    /// <summary>
    /// 默认数据库种子迁移接口
    /// </summary>
    public interface IDefaultSeedAction : ISeedAction<DefaultDbContext>
    {
    }
}
