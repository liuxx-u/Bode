using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;

namespace Bode.Services.Core.Models.User
{
    [Generate]
    [Description("用户-消息反馈")]
    public class FeedBack : EntityBase<int>
    {
        [Display(Name = @"反馈内容")]
        public string Content { get; set; }

        [Display(Name = @"用户信息")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
