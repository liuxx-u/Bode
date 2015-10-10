// -----------------------------------------------------------------------
//  <copyright file="IdentityService.Role.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-03-24 17:10</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bode.Services.Core.Dtos.Identity;
using Bode.Services.Core.Models.Identity;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;

namespace Bode.Services.Implement.Services
{
    public partial class IdentityService
    {
        #region Implementation of IIdentityContract

        /// <summary>
        /// 获取 角色信息查询数据集
        /// </summary>
        public IQueryable<SysRole> Roles
        {
            get { return RoleRepository.Entities; }
        }
        
        /// <summary>
        /// 保存角色信息信息
        /// </summary>
        /// <param name="dtos">待保存的角色DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SaveRoles(params SysRoleDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            var addDtos = dtos.Where(p => p.Id == 0).ToArray();
            var updateDtos = dtos.Where(p => p.Id > 0).ToArray();

            try
            {
                //dto检查委托
                Action<SysRoleDto> checkAction =dto =>
                {
                    if (dto.OrganizationId.HasValue)
                    {
                        if (RoleRepository.CheckExists(m => m.Name == dto.Name && m.Organization != null && m.Organization.Id == dto.OrganizationId.Value, dto.Id))
                        {
                            throw new Exception("同组织机构中名称为“{0}”的角色已存在，不能重复添加。".FormatWith(dto.Name));
                        }
                    }
                    else if (RoleRepository.CheckExists(m => m.Name == dto.Name && m.Organization == null, dto.Id))
                    {
                        throw new Exception("无组织机构的名称为的角色已存在，不能重复添加".FormatWith(dto.Name));
                    }
                };

                //dto更新委托
                Func<SysRoleDto, SysRole, SysRole> updateFunc = (dto, entity) =>
                {
                    if (dto.OrganizationId.HasValue && dto.OrganizationId.Value > 0)
                    {
                        SysOrganization organization = OrganizationRepository.GetByKey(dto.OrganizationId.Value);
                        if (organization == null)
                        {
                            throw new Exception("要加入的组织机构不存在。");
                        }
                        entity.Organization = organization;
                    }
                    else
                    {
                        entity.Organization = null;
                    }
                    return entity;
                };

                RoleRepository.UnitOfWork.TransactionEnabled = true;
                if (addDtos.Length > 0)
                {
                    RoleRepository.Insert(addDtos, checkAction, updateFunc);
                }
                if (updateDtos.Length > 0)
                {
                    RoleRepository.Update(updateDtos, checkAction, updateFunc);
                }
                await RoleRepository.UnitOfWork.SaveChangesAsync();
                return new OperationResult(OperationResultType.Success, "保存成功");
            }
            catch(Exception e)
            {
                return new OperationResult(OperationResultType.Error, e.Message);
            }
        }

        /// <summary>
        /// 删除角色信息信息
        /// </summary>
        /// <param name="ids">要删除的角色信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteRoles(params int[] ids)
        {
            return await Task.Run(() => RoleRepository.Delete(ids));
        }

        #endregion
    }
}