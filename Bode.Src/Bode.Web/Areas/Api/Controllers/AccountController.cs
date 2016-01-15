using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.User;
using OSharp.Core.Context;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Web.Http.Messages;
using OSharp.Web.Http;
using System.ComponentModel;
using OSharp.Web.Http.Authentication;
using System.Configuration;
using OSharp.Core.Data.Extensions;

namespace Bode.Web.Areas.Api.Controllers
{
    [Description("账户接口")]
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
        [Description("获取邮箱验证码")]
        public async Task<IHttpActionResult> GetEmailCode(string email, CodeType codeType = CodeType.用户注册)
        {
            if (!email.IsEmail()) return Json(new ApiResult(OperationResultType.ValidError, "请输入正确的邮箱地址"));

            var result = await UserContract.GetEmailValidateCode(email, codeType);
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
        public async Task<IHttpActionResult> Login(string userName, string password, LoginDevice loginDevice, string clientVersion, string registKey = "")
        {
            var result = await UserContract.Login(userName, password, registKey, loginDevice, clientVersion);
            return Json(result.ToApiResult());
        }

        [HttpPost]
        [TokenAuth]
        [Description("启动时重置Token过期时间")]
        public async Task<IHttpActionResult> ResetTokenValidityPeriod(LoginDevice loginDevice, string clientVersion)
        {
            var user = await UserContract.UserInfos.SingleOrDefaultAsync(p => p.Id == OperatorId);
            if (user == null) return Json(new ApiResult(OperationResultType.QueryNull, "用户不存在"));
            if (user.SysUser.IsLocked) return Json(new ApiResult(OperationResultType.NoChanged, "用户已被冻结,请联系客服。"));

            if (loginDevice == LoginDevice.Android && clientVersion != ConfigurationManager.AppSettings["ApkVision"])
            {
                return Json(new ApiResult(OperationResultType.ValidError, "有新版本，请更新。"));
            }
            if (loginDevice == LoginDevice.Ios && clientVersion != ConfigurationManager.AppSettings["IpaVision"])
            {
                return Json(new ApiResult(OperationResultType.ValidError, "有新版本，请更新。"));
            }

            var result = await UserContract.ResetToken(user, loginDevice, clientVersion);
            return Json(result.ToApiResult());
        }

        [HttpPost]
        [Description("重置密码")]
        public async Task<IHttpActionResult> ResetPassword(string userName, string newPsw, string validateCode)
        {
            var result = await UserContract.ResetPassword(userName, newPsw, validateCode);
            return Json(result.ToApiResult());
        }

        [HttpPost]
        [TokenAuth]
        [Description("修改密码")]
        public async Task<IHttpActionResult> ChangePassword(string userName, string oldPsw, string newPsw)
        {
            var result = await UserContract.ChangePassword(userName, oldPsw, newPsw);
            return Json(result.ToApiResult());
        }

        [HttpPost]
        [TokenAuth]
        [Description("修改用户名")]
        public async Task<IHttpActionResult> ChangeUserName(string userName, string newUserName, string password, string validateCode)
        {
            var result = await UserContract.ChangeUserName(userName, newUserName, password, validateCode);
            return Json(result.ToApiResult());
        }

        [HttpPost]
        [TokenAuth]
        [Description("编辑用户信息")]
        public async Task<IHttpActionResult> EditUserInfo(UserInfoEditDto dto)
        {
            if (dto.Id != OperatorId)
            {
                return Json(new ApiResult(OperationResultType.ValidError, "身份信息错误"));
            }

            var result = await UserContract.EditUserInfos(dto);
            return Json(result.ToApiResult());
        }

        [HttpPost]
        [TokenAuth]
        [Description("获取自己个人信息")]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            var theUser = await UserContract.UserInfos.Unrecycled().Include(p => p.SysUser).SingleOrDefaultAsync(p => p.Id == OperatorId);
            if (theUser == null) return Json(new ApiResult(OperationResultType.QueryNull, "用户不存在"));

            var userData = new
            {
                PhoneNo = theUser.SysUser.UserName
            };
            return Json(new ApiResult("获取成功", userData));
        }

        [HttpPost]
        [TokenAuth]
        [Description("意见反馈")]
        public async Task<IHttpActionResult> AddFeedBack(string content)
        {
            var dto = new FeedBackDto { UserInfoId = OperatorId, Content = content };
            var result = await UserContract.SaveFeedBacks(dtos: dto);

            return Json(new ApiResult(result.ResultType, "反馈成功"));
        }

        #endregion
    }
}
