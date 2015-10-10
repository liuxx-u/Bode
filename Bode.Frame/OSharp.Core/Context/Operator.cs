// -----------------------------------------------------------------------
//  <copyright file="Operator.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-08-12 19:24</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace OSharp.Core.Context
{
    /// <summary>
    /// 当前操作者信息类
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// 获取或设置 当前用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 获取或设置 当前用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// 获取或设置 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 登录设备
        /// </summary>
        public LoginDevice LoginDevice { get; set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public Dictionary<string, object> UserDatas { get; set; }

        /// <summary>
        /// 操作Ip地址
        /// </summary>
        public string Ip { get; set; }
    }

    /// <summary>
    /// 登录设备
    /// </summary>
    public enum LoginDevice
    {
        /// <summary>
        /// Android
        /// </summary>
        Android = 1,

        /// <summary>
        /// IOS
        /// </summary>
        Ios = 2,

        /// <summary>
        /// web登录
        /// </summary>
        Web = 3
    }
}