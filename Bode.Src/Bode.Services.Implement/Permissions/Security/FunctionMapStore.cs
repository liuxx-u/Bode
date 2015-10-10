// -----------------------------------------------------------------------
//  <copyright file="FunctionMapStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-08-07 3:53</last-date>
// -----------------------------------------------------------------------

using System;
using Bode.Services.Core.Dtos.Security;
using Bode.Services.Core.Models.Identity;
using Bode.Services.Core.Models.Security;
using OSharp.Core.Security;

namespace Bode.Services.Implement.Permissions.Security
{
    /// <summary>
    /// 功能（角色、用户）映射存储
    /// </summary>
    public class FunctionMapStore
        : FunctionMapStoreBase<FunctionRoleMap, int, FunctionUserMap, int, FunctionRoleMapDto, FunctionUserMapDto, Function, Guid, SysRole, int, SysUser, int>
    { }
}