using OSharp.Core.Data;
using System.ComponentModel;

namespace Bode.Services.Core.Models
{
    /// <summary>
    /// Bannner基类，分图片和视频
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    public abstract class BannerEntityBase<Tkey> : EntityBase<Tkey>
    {
        [Description("排序号")]
        public int OrderNo { get; set; }

        [Description("地址")]
        public string Path { get; set; }

        [Description("是否显示")]
        public bool IsDisplay { get; set; }

        [Description("封面")]
        public string Cover { get; set; }

        [Description("Banner类型")]
        public BannerType BannerType { get; set; }
    }

    public enum BannerType
    {
        不限 = 0,
        图片 = 1,
        视频 = 2
    }
}
