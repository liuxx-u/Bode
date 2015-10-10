// -----------------------------------------------------------------------
//  <copyright file="UserStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-08-03 20:54</last-date>
// -----------------------------------------------------------------------

using Bode.Services.Core.Models.Identity;
using OSharp.Core.Identity;

namespace Bode.Services.Implement.Permissions.Identity
{
    /// <summary>
    /// 用户存储实现
    /// </summary>
    public class UserStore : UserStoreBase<SysUser, int, SysRole, int, SysUserRoleMap, int, SysUserLogin, int, SysUserClaim, int>
    { }
}