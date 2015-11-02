// -----------------------------------------------------------------------
//  <copyright file="UserManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-08-03 11:47</last-date>
// -----------------------------------------------------------------------

using Bode.Services.Core.Models.Identity;
using Microsoft.AspNet.Identity;
using OSharp.Core.Dependency;

namespace Bode.Services.Implement.Permissions.Identity
{
    /// <summary>
    /// 用户管理器
    /// </summary>
    public class UserManager : UserManager<SysUser, int>, IScopeDependency
    {
        /// <summary>
        /// 初始化一个<see cref="UserManager"/>类型的新实例
        /// </summary>
        public UserManager(IUserStore<SysUser, int> store)
            : base(store)
        { }
    }
}