using System.ComponentModel;
using OSharp.Core.Identity.Models;

namespace Bode.Services.Core.Models.Identity
{
    /// <summary>
    /// 实体类——系统角色
    /// </summary>
    [Description("认证-系统角色")]
    public class SysRole : RoleBase<int>
    {
        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        public string test { get; set; }

        /// <summary>
        /// 获取或设置 角色所属组织机构
        /// </summary>
        public virtual SysOrganization Organization { get; set; }
    }
}