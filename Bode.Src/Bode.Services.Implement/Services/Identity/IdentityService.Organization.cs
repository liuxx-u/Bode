// -----------------------------------------------------------------------
//  <copyright file="IdentityService.SysOrganization.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-03-24 17:09</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 获取 组织机构信息查询数据集
        /// </summary>
        public IQueryable<SysOrganization> Organizations
        {
            get { return OrganizationRepository.Entities; }
        }

        /// <summary>
        /// 保存组织机构信息
        /// </summary>
        /// <param name="dtos">待保存的组织机构Dto信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SaveOrganizations(params SysOrganizationDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            var addDtos = dtos.Where(p => p.Id == 0).ToArray();
            var updateDtos = dtos.Where(p => p.Id > 0).ToArray();
            List<SysOrganization> saveOrganizations = new List<SysOrganization>();
            try
            {
                //dto检查委托
                Action<SysOrganizationDto> checkAction = dto =>
                {
                    if (OrganizationRepository.CheckExists(m => m.Name == dto.Name, dto.Id))
                    {
                        throw new Exception("名称为“{0}”的组织机构已存在，不能重复添加。".FormatWith(dto.Name));
                    }
                };

                //dto更新委托
                Func<SysOrganizationDto, SysOrganization, SysOrganization> updateFunc = (dto, entity) =>
                {
                    if (!dto.ParentId.HasValue || dto.ParentId == 0)
                    {
                        entity.Parent = null;
                    }
                    else if (entity.Parent == null || entity.Parent.Id != dto.ParentId)
                    {
                        var parentId = dto.ParentId.Value;
                        SysOrganization parent = OrganizationRepository.GetByKey(parentId);
                        if (parent != null)
                        {
                            entity.Parent = parent;
                        }
                    }
                    saveOrganizations.Add(entity);
                    return entity;
                };

                OrganizationRepository.UnitOfWork.TransactionEnabled = true;
                if (addDtos.Length > 0)
                {
                    OrganizationRepository.Insert(addDtos, checkAction, updateFunc);
                }
                if (updateDtos.Length > 0)
                {
                    OrganizationRepository.Update(updateDtos, checkAction, updateFunc);
                }
                await RoleRepository.UnitOfWork.SaveChangesAsync();

                int[] ids = saveOrganizations.Select(m => m.Id).ToArray();
                //RefreshSysOrganizationsTreePath(ids);
                return new OperationResult(OperationResultType.Success, "保存成功");
            }
            catch (Exception e)
            {
                return new OperationResult(OperationResultType.Error, e.Message);
            }
        }

        /// <summary>
        /// 删除组织机构信息信息
        /// </summary>
        /// <param name="ids">要删除的组织机构信息编号</param>
        /// <returns>业务操作结果</returns>
        public OperationResult DeleteOrganizations(params int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = OrganizationRepository.Delete(ids,
                entity =>
                {
                    if (entity.Children.Any())
                    {
                        throw new Exception("组织机构“{0}”的子级不为空，不能删除。".FormatWith(entity.Name));
                    }
                });
            return result;
        }

        #endregion

        #region 私有方法

        private void RefreshSysOrganizationsTreePath(params int[] ids)
        {
            if (ids.Length == 0)
            {
                return;
            }
            List<SysOrganization> SysOrganizations = OrganizationRepository.GetInclude(m => m.Parent).Where(m => ids.Contains(m.Id)).ToList();
            OrganizationRepository.UnitOfWork.TransactionEnabled = true;
            foreach (SysOrganization SysOrganization in SysOrganizations)
            {
                SysOrganization.TreePath = SysOrganization.Parent == null
                    ? SysOrganization.Id.ToString()
                    : SysOrganization.Parent.TreePathIds.Union(new[] { SysOrganization.Id }).ExpandAndToString();
                OrganizationRepository.Update(SysOrganization);
            }
            OrganizationRepository.UnitOfWork.SaveChanges();
        }

        #endregion
    }
}