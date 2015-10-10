using System.ComponentModel;
using OSharp.Core.Identity.Models;

namespace Bode.Services.Core.Models.Identity
{
    /// <summary>
    /// 实体类——用户角色映射
    /// </summary>
    [Description("认证-用户角色映射")]
    public class SysUserRoleMap : UserRoleMapBase<int, SysUser, int, SysRole, int>
    { }
}