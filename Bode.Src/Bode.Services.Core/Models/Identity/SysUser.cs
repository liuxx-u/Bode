using System.ComponentModel;
using OSharp.Core.Identity.Models;

namespace Bode.Services.Core.Models.Identity
{
    /// <summary>
    /// 实体类——系统用户
    /// </summary>
    [Description("认证-系统用户")]
    public class SysUser : UserBase<int>
    {
        /// <summary>
        /// 获取或设置 是否冻结
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 用户类型
        /// </summary>
        public UserType UserType { get; set; }
    }

    public enum UserType
    {
        系统用户,
        渠道用户,
        App用户
    }
}