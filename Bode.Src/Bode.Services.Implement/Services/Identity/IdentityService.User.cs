// -----------------------------------------------------------------------
//  <copyright file="IdentityService.User.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-03-24 17:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bode.Services.Core.Dtos.Identity;
using Bode.Services.Core.Models.Identity;
using Microsoft.AspNet.Identity;

using OSharp.Data.Entity;
using OSharp.Core.Identity;
using Bode.Services.Implement.Permissions.Identity;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;


namespace Bode.Services.Implement.Services
{
    public partial class IdentityService
    {
        #region Implementation of IIdentityContract

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<SysUser> Users
        {
            get { return UserRepository.Entities; }
        }

        /// <summary>
        /// 获取或设置 用户管理器
        /// </summary>
        public UserManager UserManager { get; set; }

        /// <summary>
        /// 保存用户信息信息
        /// </summary>
        /// <param name="dtos">要保存的用户DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SaveUsers(params SysUserDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            UserRepository.UnitOfWork.TransactionEnabled = true;
            foreach (SysUserDto dto in dtos)
            {
                IdentityResult result;
                SysUser user = new SysUser();
                if (dto.Id > 0)//处理编辑
                {
                    user = UserManager.FindById(dto.Id);
                    if (user == null)
                    {
                        return new OperationResult(OperationResultType.QueryNull);
                    }
                }
                user = dto.MapTo(user);
                //密码单独处理
                if (!dto.Password.IsNullOrEmpty())
                {
                    result = await UserManager.PasswordValidator.ValidateAsync(dto.Password);
                    if (!result.Succeeded)
                    {
                        return result.ToOperationResult();
                    }
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(dto.Password);
                }
                result = dto.Id > 0 ? await UserManager.UpdateAsync(user) : await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return new OperationResult(OperationResultType.Error, result.Errors.ExpandAndToString());
                }
            }
            return UserRepository.UnitOfWork.SaveChanges() > 0
                ? new OperationResult(OperationResultType.Success,"保存成功")
                : OperationResult.NoChanged;
        }

        /// <summary>
        /// 删除用户信息信息
        /// </summary>
        /// <param name="ids">要删除的用户信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteUsers(params int[] ids)
        {
            OperationResult result = UserRepository.Delete(ids,null,
                entity =>
                {
                    //先删除所有用户相关信息
                    //UserExtendRepository.Delete(entity.Extend);
                    return entity;
                });
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 设置用户的角色
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="roleIds">角色编号集合</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetUserRoles(int id, int[] roleIds)
        {
            SysUser user = await UserRepository.GetByKeyAsync(id);
            if (user == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定编号的用户信息不存在");
            }
            int[] existIds = UserRoleMapRepository.Entities.Where(m => m.User.Id == id).Select(m => m.Role.Id).ToArray();
            int[] addIds = roleIds.Except(existIds).ToArray();
            int[] removeIds = existIds.Except(roleIds).ToArray();
            UserRoleMapRepository.UnitOfWork.TransactionEnabled = true;
            foreach (int addId in addIds)
            {
                SysRole role = await RoleRepository.GetByKeyAsync(addId);
                if (role == null)
                {
                    return new OperationResult(OperationResultType.QueryNull, "指定编号的角色信息不存在");
                }
                SysUserRoleMap map = new SysUserRoleMap() { User = user, Role = role };
                await UserRoleMapRepository.InsertAsync(map);
            }
            await UserRoleMapRepository.DeleteAsync(m => m.User.Id == id && removeIds.Contains(m.Role.Id));
            return await UserRoleMapRepository.UnitOfWork.SaveChangesAsync() > 0
                ? new OperationResult(OperationResultType.Success, "用户“{0}”指派角色操作成功".FormatWith(user.UserName))
                : OperationResult.NoChanged;
        }

        #endregion
    }
}