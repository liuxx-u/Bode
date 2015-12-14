// -----------------------------------------------------------------------
//  <copyright file="OSharpContext.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-07-28 0:41</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;
using OSharp.Core.Security;
using OSharp.Utility;


namespace OSharp.Core.Context
{
    /// <summary>
    /// OSharp框架上下文，用于构造OSharp框架运行环境
    /// </summary>
    [Serializable]
    public class OSharpContext : Dictionary<string, object>
    {
        private const string OperatorKey = "__OSharp_Context_Operator";
        private const string CultureKey = "__OSharp_Context_Culture";
        private static readonly Lazy<OSharpContext> ContextLazy = new Lazy<OSharpContext>(() => new OSharpContext());

        private OSharpContext()
        { }

        /// <summary>
        /// 获取 当前上下文
        /// </summary>
        public static OSharpContext Current
        {
            get { return ContextLazy.Value; }
        }

        /// <summary>
        /// 获取 当前操作者
        /// </summary>
        public Operator Operator
        {
            get
            {
                if (!ContainsKey(OperatorKey))
                {
                    this[OperatorKey] = new Operator();
                }
                return this[OperatorKey] as Operator;
            }
            private set
            {
                this[OperatorKey] = value;
            }
        }
        
        /// <summary>
        /// 设置当前操作者信息
        /// </summary>
        public void SetOperator(ClaimsPrincipal user)
        {
            Operator @operator = Operator;
        }

        /// <summary>
        /// 设置当前操作者对象
        /// </summary>
        /// <param name="user"></param>
        public void SetOperator(Operator user)
        {
            user.CheckNotNull("user");
            Operator = user;
        }
        
        /// <summary>
        /// 获取或设置 功能信息处理器
        /// </summary>
        public IFunctionHandler FunctionHandler { get; set; }

        /// <summary>
        /// 获取或设置 实体数据信息处理器
        /// </summary>
        public IEntityInfoHandler EntityInfoHandler { get; set; }
    }
}