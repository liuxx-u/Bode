// -----------------------------------------------------------------------
//  <copyright file="UserRoleMapConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-06-17 23:02</last-date>
// -----------------------------------------------------------------------

using System;
using Bode.Services.Core.Models.Identity;
using OSharp.Data.Entity;

namespace Bode.Services.Implement.ModelConfigs.Identity
{
    public class SysUserRoleMapConfiguration : EntityConfigurationBase<SysUserRoleMap, Int32>
    {
        /// <summary>
        /// 初始化一个<see cref="SysUserRoleMapConfiguration"/>类型的新实例
        /// </summary>
        public SysUserRoleMapConfiguration()
        {
            HasRequired(m => m.User).WithMany();
            HasRequired(m => m.Role).WithMany();
        }
    }
}