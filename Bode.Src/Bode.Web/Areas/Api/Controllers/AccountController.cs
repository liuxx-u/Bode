using System.ComponentModel;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.User;
using OSharp.Core.Context;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Web.Http;
using OSharp.Web.Http.Auth;
using OSharp.Web.Http.Messages;

namespace Bode.Web.Areas.Api.Controllers
{
    [Description("账户相关接口")]
    public class AccountController : BaseApiController
    {
        public IUserContract UserContract { get; set; }

        #region 账户相关业务

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <param name="codeType">1:注册;2:修改密码;3:修改手机号</param>
        [HttpPost]
        [Description("获取短信验证码")]
        public async Task<IHttpActionResult> GetSmsCode(string phoneNo, CodeType codeType = CodeType.用户注册)
        {
            if (!phoneNo.IsMobileNumber(true)) return Json(new ApiResult(OperationResultType.ValidError, "请输入正确的手机号"));
            var result = await UserContract.GetSmsValidateCode(phoneNo, codeType);

            return Json(result.ToApiResult());
        }

        [HttpPost]
        [Description("验证并注册")]
        public async Task<IHttpActionResult> ValidateRegister(UserInfoRegistDto dto, string validateCode)
        {
            var result = await UserContract.ValidateRegister(dto, validateCode);
            return Json(result.ToApiResult());
        }

        [HttpPost]
        [Description("用户登录")]
        public async Task<IHttpActionResult> Login(string phoneNo, string password, LoginDevice loginDevice, string registKey = "")
        {
            var result = await UserContract.Login(phoneNo, password, registKey, loginDevice);
            return Json(result.ToApiResult());
        }

        [ApiAuth]
        [HttpPost]
        [Description("重置密码")]
        public async Task<IHttpActionResult> ResetPassword(string phoneNo, string newPsw, string validateCode)
        {
            var result = await UserContract.ResetPassword(phoneNo, newPsw, validateCode);
            return Json(result.ToApiResult());
        }

        [ApiAuth]
        [HttpPost]
        [Description("修改密码")]
        public async Task<IHttpActionResult> ChangePassword(string phoneNo, string oldPsw, string newPsw)
        {
            var result = await UserContract.ChangePassword(phoneNo, oldPsw, newPsw);
            return Json(result.ToApiResult());
        }

        [ApiAuth]
        [HttpPost]
        [Description("修改手机号")]
        public async Task<IHttpActionResult> ChangePhoneNo(string phoneNo, string newPhoneNo, string password, string validateCode)
        {
            var result = await UserContract.ChangePhoneNo(phoneNo, newPhoneNo, password, validateCode);
            return Json(result.ToApiResult());
        }

        [ApiAuth]
        [HttpPost]
        [Description("编辑用户信息")]
        public async Task<IHttpActionResult> EditUserInfo(string data)
        {
            var dto = JsonToEntity<UserInfoEditDto>(data);
            var result = await UserContract.EditUserInfos(dto);
            return Json(new ApiResult(result.ResultType, "信息更新成功"));
        }

        [ApiAuth]
        [HttpPost]
        [Description("获取自己个人信息")]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            var theUser = await UserContract.UserInfos.SingleOrDefaultAsync(p => p.Id == OperatorId);
            if (theUser == null) return Json(new ApiResult(OperationResultType.QueryNull, "用户不存在"));

            var userData = new
            {
                theUser.Id,
                theUser.SysUser.NickName,
                theUser.Signature,
                theUser.HeadPic,
                theUser.Sex,
                theUser.BirthDay
            };
            return Json(new ApiResult("获取成功", userData));
        }

        [ApiAuth]
        [HttpPost]
        [Description("意见反馈")]
        public async Task<IHttpActionResult> AddFeedBack(FeedBackDto dto)
        {
            dto.UserInfoId = OperatorId;
            var result = await UserContract.SaveFeedBacks(dtos: dto);

            return Json(new ApiResult(result.ResultType, "反馈成功"));
        }

        #endregion
    }
}
