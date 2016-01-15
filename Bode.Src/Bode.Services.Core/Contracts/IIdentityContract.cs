// -----------------------------------------------------------------------
//  <copyright file="IIdentityContract.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-03-24 16:13</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using OSharp.Core;
using OSharp.Core.Dependency;
using Bode.Services.Core.Dtos.Identity;
using Bode.Services.Core.Models.Identity;
using OSharp.Utility.Data;


namespace Bode.Services.Core.Contracts
{
    /// <summary>
    /// 业务契约——身份认证模块
    /// </summary>
    public interface IIdentityContract : IScopeDependency
    {
        /// <summary>
        /// 获取 用户角色映射数据集
        /// </summary>
        IQueryable<SysUserRoleMap> UserRoleMaps { get;}

        #region 组织机构信息业务

        /// <summary>
        /// 获取 组织机构信息查询数据集
        /// </summary>
        IQueryable<SysOrganization> Organizations { get; }

        /// <summary>
        /// 保存组织机构信息
        /// </summary>
        /// <param name="dtos">待保存的组织机构Dto信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> SaveOrganizations(params SysOrganizationDto[] dtos);

        /// <summary>
        /// 删除组织机构信息信息
        /// </summary>
        /// <param name="ids">要删除的组织机构信息编号</param>
        /// <returns>业务操作结果</returns>
        OperationResult DeleteOrganizations(params int[] ids);

        #endregion

        #region 角色信息业务

        /// <summary>
        /// 获取 角色信息查询数据集
        /// </summary>
        IQueryable<SysRole> Roles { get; }

        /// <summary>
        /// 保存角色信息信息
        /// </summary>
        /// <param name="dtos">待保存的角色DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> SaveRoles(params SysRoleDto[] dtos);

        /// <summary>
        /// 删除角色信息信息
        /// </summary>
        /// <param name="ids">要删除的角色信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteRoles(params int[] ids);

        #endregion

        #region 用户信息业务

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        IQueryable<SysUser> Users { get; }

        /// <summary>
        /// 冻结/解冻用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> LockUserOrNot(int userId);

        /// <summary>
        /// 冻结/解冻用户
        /// </summary>
        /// <param name="user">系统用户</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> LockUserOrNot(SysUser user);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="oldPsw">原密码</param>
        /// <param name="newPsw">新密码</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> ResetPassword(string userName, string oldPsw, string newPsw);


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">用户密码</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Login(string userName, string password);

        /// <summary>
        /// 保存用户信息信息
        /// </summary>
        /// <param name="dtos">要保存的用户DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> SaveUsers(params SysUserDto[] dtos);

        /// <summary>
        /// 删除用户信息信息
        /// </summary>
        /// <param name="ids">要删除的用户信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteUsers(params int[] ids);

        /// <summary>
        /// 设置用户的角色
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="roleIds">角色编号集合</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> SetUserRoles(int id, int[] roleIds);

        #endregion
    }
}