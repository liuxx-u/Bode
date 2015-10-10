﻿// -----------------------------------------------------------------------
//  <copyright file="Extensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-09-23 17:37</last-date>
// -----------------------------------------------------------------------

using System.Collections;
using System.Web.Mvc;

using OSharp.Core;
using OSharp.Core.Context;
using OSharp.Core.Data;
using OSharp.Core.Security;
using OSharp.Web.Mvc.UI;


namespace OSharp.Web.Mvc.Extensions
{
    /// <summary>
    /// 扩展辅助操作方法
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 获取MVC操作的相关功能信息
        /// </summary>
        public static IFunction GetExecuteFunction(this ControllerContext context)
        {
            const string key = Constants.CurrentMvcFunctionKey;
            IDictionary items = context.HttpContext.Items;
            if (items.Contains(key))
            {
                return (IFunction)items[key];
            }
            string area = context.GetAreaName();
            string controller = context.GetControllerName();
            string action = context.GetActionName();
            IFunction function = OSharpContext.Current.FunctionHandler.GetFunction(area, controller, action, "MVC");
            if (function != null)
            {
                items.Add(key, function);
            }
            return function;
        }

        /// <summary>
        /// 获取MVC操作的相关功能信息
        /// </summary>
        public static IFunction GetExecuteFunction(this ControllerBase controller)
        {
            return controller.ControllerContext.GetExecuteFunction();
        }

        /// <summary>
        /// 将分页数据转换为表格数据格式
        /// </summary>
        public static GridData<TData> ToGridData<TData>(this PageResult<TData> pageResult)
        {
            return new GridData<TData>(pageResult.Data, pageResult.Total);
        }
    }
}