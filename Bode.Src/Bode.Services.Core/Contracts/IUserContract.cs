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
        /// 获取邮箱验证码
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="codeType">验证码类型</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> GetEmailValidateCode(string email, CodeType codeType);

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
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="registKey">极光注册Key</param>
        /// <param name="loginDevice">登录设备</param>
        /// <param name="clientVersion">客户端当前版本</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Login(string userName, string password, string registKey, LoginDevice loginDevice, string clientVersion);


        /// <summary>
        /// 重置用户Token有效期
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="loginDevice">登录设备</param>
        /// <param name="clientVersion">客户端版本</param>
        /// <returns></returns>
        Task<OperationResult> ResetToken(UserInfo user, LoginDevice loginDevice, string clientVersion);

        /// <summary>
        /// 更改用户名
        /// </summary>
        /// <param name="userName">原用户名</param>
        /// <param name="newUserName">新用户名</param>
        /// <param name="password">登录密码</param>
        /// <param name="validateCode">验证码</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> ChangeUserName(string userName, string newUserName, string password, string validateCode);


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="oldPsw">原密码</param>
        /// <param name="newPsw">新密码</param>
        /// <returns></returns>
        Task<OperationResult> ChangePassword(string userName, string oldPsw, string newPsw);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userName">电话号码</param>
        /// <param name="newPsw">新密码</param>
        /// <param name="validateCode">验证码</param>
        /// <returns></returns>
        Task<OperationResult> ResetPassword(string userName, string newPsw, string validateCode);

        #endregion

        /// <summary>
        /// 编辑UserInfo信息
        /// </summary>
        /// <param name="dtos">要更新的UserInfoEditDto信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditUserInfos(params UserInfoEditDto[] dtos);
    }
}
