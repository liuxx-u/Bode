using System.Threading.Tasks;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.User;
using OSharp.Core.Context;
using OSharp.Utility.Data;

namespace Bode.Services.Core.Contracts
{
    public partial interface IUserContract
    {
        #region 账户相关

        /// <summary>
        /// 获取用户注册验证码
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <param name="codeType">验证码类型</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> GetSmsValidateCode(string phoneNo, CodeType codeType);

        /// <summary>
        /// 验证用户注册
        /// </summary>
        /// <param name="dto">注册信息</param>
        /// <param name="validateCode">验证码</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> ValidateRegister(UserInfoRegistDto dto, string validateCode);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <param name="password">密码</param>
        /// <param name="registKey">极光注册Key</param>
        /// <param name="loginDevice">登录设备</param>
        /// <returns></returns>
        Task<OperationResult> Login(string phoneNo, string password, string registKey, LoginDevice loginDevice);

        /// <summary>
        /// 更改手机号
        /// </summary>
        /// <param name="phoneNo">原手机号</param>
        /// <param name="newPhoneNo">新手机号</param>
        /// <param name="password">登录密码</param>
        /// <param name="validateCode">验证码</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> ChangePhoneNo(string phoneNo, string newPhoneNo, string password, string validateCode);


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <param name="oldPsw">原密码</param>
        /// <param name="newPsw">新密码</param>
        /// <returns></returns>
        Task<OperationResult> ChangePassword(string phoneNo, string oldPsw, string newPsw);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="phoneNo">电话号码</param>
        /// <param name="newPsw">新密码</param>
        /// <param name="validateCode">验证码</param>
        /// <returns></returns>
        Task<OperationResult> ResetPassword(string phoneNo, string newPsw, string validateCode);

        #endregion

        /// <summary>
        /// 编辑UserInfo信息
        /// </summary>
        /// <param name="dtos">要更新的UserInfoEditDto信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditUserInfos(params UserInfoEditDto[] dtos);
    }
}
