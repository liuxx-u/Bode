using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;

namespace Bode.Services.Core.Models.User
{
    [Generate]
    [Description("用户-验证码")]
    public class ValidateCode : EntityBase<int>
    {
        [Description("电话号码")]
        public string PhoneNo { get; set; }

        [Description("验证码类型")]
        public CodeType CodeType { get; set; }

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
}
