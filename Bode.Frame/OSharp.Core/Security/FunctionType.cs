// -----------------------------------------------------------------------
//  <copyright file="FunctionType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-06-17 9:11</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace OSharp.Core.Security
{
    /// <summary>
    /// 表示功能访问类型的枚举
    /// </summary>
    public enum FunctionType
    {
        /// <summary>
        /// 匿名用户可访问
        /// </summary>
        [Description("匿名访问")]
        Anonymouse = 0,

        /// <summary>
        /// 登录用户可访问
        /// </summary>
        [Description("登陆访问")]
        Logined = 1,

        /// <summary>
        /// 指定角色可访问
        /// </summary>
        [Description("角色访问")]
        RoleLimit = 2
    }
}