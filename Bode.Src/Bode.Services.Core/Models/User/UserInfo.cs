using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bode.Services.Core.Models.Identity;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;

namespace Bode.Services.Core.Models.User
{
    [Generate]
    [Description("用户-用户信息")]
    public class UserInfo : EntityBase<int>
    {
        [NotRepeat(GroupName = "手机号"), Required, Description("手机号")]
        public string PhoneNo { get; set; }

        [Description("登录密码")]
        public string Password { get; set; }

        [Description("昵称")]
        public string NickName { get; set; }

        [Description("头像地址")]
        public string HeadPic { get; set; }

        [Description("出生年月")]
        public DateTime BirthDay { get; set; }

        [Description("个性签名")]
        public string Signature { get; set; }

        [Description("设备唯一标识号")]
        public string RegistKey { get; set; }

        [Description("性别")]
        public Sex Sex { get; set; }

        [Description("系统用户")]
        public virtual SysUser SysUser { get; set; }

        /// <summary>
        /// 登录凭据
        /// </summary>
        public string Token { get; set; }
    }

    public enum Sex
    {
        不限 = 0,
        男 = 1,
        女 = 2
    }
}
