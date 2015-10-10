// -----------------------------------------------------------------------
//  <copyright file="SecurityService.EntityInfo.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-07-15 2:05</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using Bode.Services.Core.Dtos.Security;
using OSharp.Core.Context;
using OSharp.Core.Security;
using OSharp.Utility.Data;

namespace Bode.Services.Implement.Services
{
    public partial class SecurityService
    {
        #region Implementation of ISecurityContract

        /// <summary>
        /// 获取 实体数据信息查询数据集
        /// </summary>
        public IQueryable<EntityInfo> EntityInfos
        {
            get { return EntityInfoRepository.Entities; }
        }

        /// <summary>
        /// 更新实体数据信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的实体数据信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult EditEntityInfos(params EntityInfoDto[] dtos)
        {
            OperationResult result = EntityInfoRepository.Update(dtos);

            if (result.ResultType == OperationResultType.Success)
            {
                OSharpContext.Current.EntityInfoHandler.RefreshCache();
            }
            return result;
        }

        #endregion
    }
}