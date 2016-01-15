// -----------------------------------------------------------------------
//  <copyright file="AdminBaseController.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-25 2:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using OSharp.Utility.Filter;
using OSharp.Web.Mvc.UI;

using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;

namespace OSharp.Web.Mvc
{
    /// <summary>
    /// 后台管理控制器基类
    /// </summary>
    public abstract class AdminBaseController : BaseController
    {
        /// <summary>
        /// 获取表格分页数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="total">总数量</param>
        /// <param name="request">表格请求</param>
        /// <returns>查询结果</returns>
        protected virtual IQueryable<TEntity> GetQueryData<TEntity, TKey>(IQueryable<TEntity> source, out int total, GridRequest request = null)
            where TEntity : EntityBase<TKey>
        {
            if (request == null)
            {
                request = new GridRequest(Request);
            }
            Expression<Func<TEntity, bool>> predicate = FilterHelper.GetExpression<TEntity>(request.FilterGroup);
            return source.Where(predicate, request.PageCondition, out total);
        }
    }
}