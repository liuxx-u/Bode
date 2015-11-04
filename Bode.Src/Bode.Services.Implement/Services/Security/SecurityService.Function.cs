﻿// -----------------------------------------------------------------------
//  <copyright file="SecurityService.Function.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-07-14 23:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bode.Services.Core.Dtos.Security;
using Bode.Services.Core.Models.Identity;
using Bode.Services.Core.Models.Security;
using OSharp.Core.Context;
using OSharp.Data.Entity;
using OSharp.Core.Security;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;

namespace Bode.Services.Implement.Services
{
    public partial class SecurityService
    {
        #region Implementation of ISecurityContract

        /// <summary>
        /// 获取 功能信息查询数据集
        /// </summary>
        public IQueryable<Function> Functions
        {
            get { return FunctionRepository.Entities; }
        }

        /// <summary>
        /// 检查功能信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的功能信息编号</param>
        /// <returns>功能信息是否存在</returns>
        public bool CheckFunctionExists(Expression<Func<Function, bool>> predicate, Guid id = new Guid())
        {
            return FunctionRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 添加功能信息信息
        /// </summary>
        /// <param name="dtos">要添加的功能信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult AddFunctions(params FunctionDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = FunctionRepository.Insert(dtos,
                dto =>
                {
                    //if (dto.Url.IsNullOrWhiteSpace())
                    //{
                    //    throw new Exception("自定义功能的URL不能为空");
                    //}
                    //if (FunctionRepository.CheckExists(m => m.Name == dto.Name))
                    //{
                    //    throw new Exception("名称为“{0}”的功能信息已存在".FormatWith(dto.Name));
                    //}
                    //if (dto.Url == null && FunctionRepository.CheckExists(m => m.Area == dto.Area && m.Controller == dto.Controller && m.Action == dto.Action))
                    //{
                    //    throw new Exception("区域“{0}”控制器“{1}”方法“{2}”的功能信息已存在".FormatWith(dto.Area, dto.Controller, dto.Action));
                    //}
                },
                (dto, entity) =>
                {
                    entity.IsCustom = true;
                    if (entity.Url.IsNullOrEmpty())
                    {
                        entity.Url = null;
                    }
                    return entity;
                });
            if (result.ResultType == OperationResultType.Success)
            {
                OSharpContext.Current.FunctionHandler.RefreshCache();
            }
            return result;
        }

        /// <summary>
        /// 更新功能信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的功能信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditFunctions(params FunctionDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            List<string> names = new List<string>();
            FunctionRepository.UnitOfWork.TransactionEnabled = true;
            foreach (FunctionDto dto in dtos)
            {
                if (FunctionRepository.CheckExists(m => m.Name == dto.Name, dto.Id))
                {
                    return new OperationResult(OperationResultType.Error, "名称为“{0}”的功能信息已存在".FormatWith(dto.Name));
                }
                Function entity = FunctionRepository.GetByKey(dto.Id);
                if (entity == null)
                {
                    return new OperationResult(OperationResultType.QueryNull);
                }
                FunctionType oldType = entity.FunctionType;
                if (dto.DataLogEnabled && !dto.OperateLogEnabled && !entity.OperateLogEnabled && !entity.DataLogEnabled)
                {
                    dto.OperateLogEnabled = true;
                }
                else if (!dto.OperateLogEnabled && dto.DataLogEnabled && entity.OperateLogEnabled && entity.DataLogEnabled)
                {
                    dto.DataLogEnabled = false;
                }
                entity = dto.MapTo(entity);
                if (entity.Url.IsNullOrEmpty())
                {
                    entity.Url = null;
                }
                if (oldType != entity.FunctionType)
                {
                    entity.IsTypeChanged = true;
                }
                FunctionRepository.Update(entity);
                names.Add(entity.Name);
            }
            int count = await FunctionRepository.UnitOfWork.SaveChangesAsync();
            OperationResult result = count > 0
                ? new OperationResult(OperationResultType.Success, "功能“{0}”更新成功".FormatWith(names.ExpandAndToString()))
                : new OperationResult(OperationResultType.NoChanged);
            if (result.ResultType == OperationResultType.Success)
            {
                OSharpContext.Current.FunctionHandler.RefreshCache();
            }
            return result;
        }

        /// <summary>
        /// 删除功能信息信息
        /// </summary>
        /// <param name="ids">要删除的功能信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteFunctions(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            List<string>names = new List<string>();
            FunctionRepository.UnitOfWork.TransactionEnabled = true;
            foreach (Guid id in ids)
            {
                Function entity = FunctionRepository.GetByKey(id);
                if (entity == null)
                {
                    return new OperationResult(OperationResultType.QueryNull);
                }
                if (!entity.IsCustom)
                {
                    return new OperationResult(OperationResultType.Error, "功能“{0}”不是自定义功能，不能删除".FormatWith(entity.Name));
                }
                FunctionRepository.Delete(entity);
                names.Add(entity.Name);
            }
            int count = await FunctionRepository.UnitOfWork.SaveChangesAsync();
            OperationResult result = count > 0
                ? new OperationResult(OperationResultType.Success, "功能“{0}”删除成功".FormatWith(names.ExpandAndToString()))
                : new OperationResult(OperationResultType.NoChanged);
            if (result.ResultType == OperationResultType.Success)
            {
                OSharpContext.Current.FunctionHandler.RefreshCache();
            }
            return result;
        }

        /// <summary>
        /// 设置角色功能
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="functionIds">功能Id集合</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetRoleFunctions(int roleId, Guid[] functionIds)
        {
            SysRole role = await RoleRepository.GetByKeyAsync(roleId);
            if (role == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定编号的角色信息不存在");
            }
            Guid[] existIds = FunctionRoleMapRepository.Entities.Where(m => m.Role.Id == roleId).Select(m => m.Function.Id).ToArray();
            Guid[] addIds = functionIds.Except(existIds).ToArray();
            Guid[] removeIds = existIds.Except(functionIds).ToArray();
            FunctionRoleMapRepository.UnitOfWork.TransactionEnabled = true;
            foreach (Guid addId in addIds)
            {
                Function function = await FunctionRepository.GetByKeyAsync(addId);
                if (function == null)
                {
                    return new OperationResult(OperationResultType.QueryNull, "指定编号的功能信息不存在");
                }
                FunctionRoleMap map = new FunctionRoleMap() { Function = function, Role = role, BeginTime = DateTime.Now };
                await FunctionRoleMapRepository.InsertAsync(map);
            }
            await FunctionRoleMapRepository.DeleteAsync(m => m.Role.Id == roleId && removeIds.Contains(m.Function.Id));
            return await FunctionRoleMapRepository.UnitOfWork.SaveChangesAsync() > 0
                ? new OperationResult(OperationResultType.Success, "角色“{0}”指派功能操作成功".FormatWith(role.Name))
                : OperationResult.NoChanged;
        }

        #endregion
    }
}