// -----------------------------------------------------------------------
//  <copyright file="UserLogin.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-06-25 14:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bode.Services.Core.Models.Identity;
using OSharp.Core.Identity.Models;


namespace Bode.Services.Core.Models.Identity
{
    /// <summary>
    /// 实体类——用户第三方登录（OAuth，如facebook,google）信息
    /// </summary>
    [Description("认证-第三方登录")]
    public class SysUserLogin : UserLoginBase<int, SysUser, int>
    { }
}