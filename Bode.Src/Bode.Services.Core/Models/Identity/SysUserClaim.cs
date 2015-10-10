// -----------------------------------------------------------------------
//  <copyright file="UserClaim.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-06-25 14:39</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using Bode.Services.Core.Models.Identity;
using OSharp.Core.Identity.Models;

namespace Bode.Services.Core.Models.Identity
{
    /// <summary>
    /// 实体类——用户摘要标识信息
    /// </summary>
    [Description("认证-用户摘要标识")]
    public class SysUserClaim : UserClaimBase<int, SysUser, int>
    { }
}