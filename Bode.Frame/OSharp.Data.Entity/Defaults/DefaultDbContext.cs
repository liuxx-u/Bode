// -----------------------------------------------------------------------
//  <copyright file="DefaultDbContext.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-06-28 3:34</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Core.Data;


namespace OSharp.Data.Entity
{
    /// <summary>
    /// 默认 EntityFramework 数据上下文
    /// </summary>
    public class DefaultDbContext : DbContextBase<DefaultDbContext>
    {
        /// <summary>
        /// 初始化一个<see cref="DefaultDbContext"/>类型的新实例
        /// </summary>
        public DefaultDbContext()
        { }

        /// <summary>
        /// 初始化一个<see cref="DefaultDbContext"/>类型的新实例
        /// </summary>
        public DefaultDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        /// <summary>
        /// 获取读库上下文
        /// </summary>
        /// <param name="readNameOrConnectionString">读库连接字符串</param>
        /// <returns>读库上下文</returns>
        protected override IUnitOfWork GetSlaveContext(string readNameOrConnectionString)
        {
            return new DefaultDbContext(readNameOrConnectionString);
        }
    }
}