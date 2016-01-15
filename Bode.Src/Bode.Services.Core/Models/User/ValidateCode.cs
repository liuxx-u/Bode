using System.ComponentModel;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;

namespace Bode.Services.Core.Models.User
{
    [Generate]
    [Description("用户-验证码")]
    public class ValidateCode : EntityBase<int>
    {
        //手机或邮箱
        [Description("验证码Key")]
        public string CodeKey { get; set; }

        [Description("验证码类型")]
        public CodeType CodeType { get; set; }

        [Description("验证方式")]
        public ValidateType ValidateType { get; set; }

        [Description("验证码")]
        public string Code { get; set; }
    }
    public enum CodeType
    {
        不限 = 0,
        用户注册 = 1,
        找回密码 = 2,
        更换手机 = 3
    }

    public enum ValidateType
    {
        不限 = 0,
        手机 = 1,
        邮箱 = 2
    }
}
