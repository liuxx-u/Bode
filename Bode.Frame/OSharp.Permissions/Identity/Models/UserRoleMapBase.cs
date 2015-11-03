// -----------------------------------------------------------------------
//  <copyright file="UserRoleMapBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-09-13 17:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

using OSharp.Core.Data;


namespace OSharp.Core.Identity.Models
{
    /// <summary>
    /// 用户角色映射信息基类
    /// </summary>
    /// <typeparam name="TKey">编号类型</typeparam>
    /// <typeparam name="TUser">用户类型</typeparam>
    /// <typeparam name="TUserKey">用户编号类型</typeparam>
    /// <typeparam name="TRole">角色类型</typeparam>
    /// <typeparam name="TRoleKey">角色编号类型</typeparam>
    public abstract class UserRoleMapBase<TKey, TUser, TUserKey, TRole, TRoleKey>
        : EntityBase<TKey>, IUserRoleMap<TKey, TUser, TUserKey, TRole, TRoleKey>
        where TUser : IUser<TUserKey>
        where TRole : IRole<TRoleKey>
    {
        private DateTime _beginTime;
        private DateTime? _endTime;

        /// <summary>
        /// 
        /// </summary>
        protected UserRoleMapBase()
        {
            _beginTime = DateTime.Now;
        }

        /// <summary>
        /// 获取或设置 生效时间
        /// </summary>
        public DateTime BeginTime
        {
            get { return _beginTime; }
            set
            {
                if (EndTime != null && value > EndTime.Value)
                {
                    throw new InvalidOperationException("生效时间不能大于过期时间");
                }
                _beginTime = value;
            }
        }

        /// <summary>
        /// 获取或设置 过期时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return _endTime; }
            set
            {
                if (value != null && value < BeginTime)
                {
                    throw new InvalidOperationException("过期时间不能小于生效时间");
                }
                _endTime = value;
            }
        }

        /// <summary>
        /// 获取或设置 用户信息
        /// </summary>
        public virtual TUser User { get; set; }

        /// <summary>
        /// 获取或设置 角色信息
        /// </summary>
        public virtual TRole Role { get; set; }
    }
}