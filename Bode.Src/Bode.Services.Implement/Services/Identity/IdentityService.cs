// -----------------------------------------------------------------------
//  <copyright file="IdentityService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-06-30 3:35</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Models.Identity;
using OSharp.Core.Data;

namespace Bode.Services.Implement.Services
{
    /// <summary>
    /// 业务实现——身份认证模块
    /// </summary>
    public partial class IdentityService : IIdentityContract
    {
        /// <summary>
        /// 获取或设置 组织机构信息仓储操作对象
        /// </summary>
        public IRepository<SysOrganization, int> OrganizationRepository { protected get; set; }

        /// <summary>
        /// 获取或设置 角色信息仓储对象
        /// </summary>
        public IRepository<SysRole, int> RoleRepository { protected get; set; }

        /// <summary>
        /// 获取或设置 用户信息仓储对象
        /// </summary>
        public IRepository<SysUser, int> UserRepository { protected get; set; }

        /// <summary>
        /// 获取或设置 用户角色映射信息仓储对象
        /// </summary>
        public IRepository<SysUserRoleMap, int> UserRoleMapRepository { get; set; }

        /// <summary>
        /// 获取 用户角色映射数据集
        /// </summary>
        public IQueryable<SysUserRoleMap> UserRoleMaps
        {
            get { return UserRoleMapRepository.Entities; }
        }
    }
}