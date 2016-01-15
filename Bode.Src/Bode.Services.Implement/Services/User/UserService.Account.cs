using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.Identity;
using Bode.Services.Core.Models.User;
using Microsoft.AspNet.Identity;
using OSharp.Core.Context;
using OSharp.Core.Identity;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Drawing;
using OSharp.Utility.Extensions;
using OSharp.Utility.Secutiry;
using OSharp.Utility.Net.Mail;
using System.Collections.Generic;

namespace Bode.Services.Implement.Services
{
    public partial class UserService
    {
        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <param name="codeType"></param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> GetSmsValidateCode(string phoneNo, CodeType codeType)
        {
            if (codeType == CodeType.用户注册)
            {
                if (UserInfoRepo.CheckExists(p => p.SysUser.UserName == phoneNo))
                {
                    return new OperationResult(OperationResultType.ValidError, "手机号已注册，不能获取注册验证码");
                } 
            }
            if (codeType == CodeType.找回密码)
            {
                if (!UserInfoRepo.CheckExists(p => p.SysUser.UserName == phoneNo))
                {
                    return new OperationResult(OperationResultType.ValidError, "该用户未注册，不能获取验证码");
                }
            }
            
            return await SendValidateCode(phoneNo, ValidateType.手机, codeType, code =>
            {
                var smsContent = "您的验证码为：" + code + "，请勿泄露。[不同]";
                Sms.Send(phoneNo, 1, smsContent);
            });
        }

        /// <summary>
        /// 获取邮箱验证码
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="codeType">验证码类型</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> GetEmailValidateCode(string email,CodeType codeType)
        {
            return await SendValidateCode(email, ValidateType.邮箱, codeType, code => 
            {
                //smtp.163.com
                string senderServerIp = "123.125.50.133";
                //smtp.gmail.com
                //string senderServerIp = "74.125.127.109";
                //string senderServerIp = "smtp.qq.com";
                //string senderServerIp = "58.251.149.147";
                //string senderServerIp = "smtp.sina.com";
                string fromMailAddress = "maixiaohao001@163.com";
                string subjectInfo = "验证码";
                string bodyInfo = "您本次的验证码为" + code + "，工作人员不会向您索要此验证码，请勿向任何人泄露。[不同]";
                string mailUsername = "maixiaohao001";
                string mailPassword = "xx"; //发送邮箱的密码
                string mailPort = "25";

                MailSender emailSender = new MailSender(senderServerIp, email, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);
                emailSender.Send();
            });
        }


        /// <summary>
        /// 验证用户注册
        /// </summary>
        /// <param name="dto">用户注册信息</param>
        /// <param name="validateCode">验证码</param>
        /// <returns>业务操作结果</returns>

        public async Task<OperationResult> ValidateRegister(UserInfoRegistDto dto, string validateCode)
        {
            dto.CheckNotNull("dto");
            validateCode.CheckNotNullOrEmpty("validateCode");
            //验证码
            var severCode = GetValidateCode(dto.UserName, CodeType.用户注册);
            if (severCode == null || severCode.Code != validateCode)
            {
                return new OperationResult(OperationResultType.ValidError, "验证码错误", 0);
            }
            if (SysUserRepo.CheckExists(p => p.UserName == dto.UserName))
            {
                return new OperationResult(OperationResultType.ValidError, "账号已被使用", 0);
            }

            try
            {
                UserInfoRepo.UnitOfWork.TransactionEnabled = true;
                //验证密码格式
                IdentityResult result = await UserManager.PasswordValidator.ValidateAsync(dto.Password);
                if (!result.Succeeded) return result.ToOperationResult();

                SysUser sUser = new SysUser()
                {
                    UserName = dto.UserName,
                    NickName = dto.NickName,
                    PasswordHash = UserManager.PasswordHasher.HashPassword(dto.Password),//密码加密
                    UserType = UserType.App用户
                };
                if (severCode.ValidateType == ValidateType.手机)
                {
                    sUser.PhoneNumber = dto.UserName;
                    sUser.PhoneNumberConfirmed = true;
                }
                else
                {
                    sUser.Email = dto.UserName;
                    sUser.EmailConfirmed = true;
                }
                await UserManager.CreateAsync(sUser);

                var userInfo = Mapper.Map<UserInfo>(dto);
                userInfo.SysUser = sUser;
                await UserInfoRepo.InsertAsync(userInfo);

                await UserInfoRepo.UnitOfWork.SaveChangesAsync();
                return new OperationResult(OperationResultType.Success, "注册成功", userInfo.Id);
            }
            catch
            {
                return new OperationResult(OperationResultType.NoChanged, "注册失败", 0);
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="registKey">极光registKey</param>
        /// <param name="loginDevice">登录设备</param>
        /// <param name="clientVersion">客户端版本</param>
        /// <returns></returns>
        public async Task<OperationResult> Login(string userName, string password, string registKey, LoginDevice loginDevice,string clientVersion)
        {
            userName.CheckNotNullOrEmpty("userName");
            password.CheckNotNullOrEmpty("password");

            SysUser sUser = await UserManager.FindByNameAsync(userName);
            var result = await UserManager.CheckPasswordAsync(sUser, password);
            if (sUser == null || sUser.UserType != UserType.App用户)
            {
                return new OperationResult(OperationResultType.QueryNull, "用户不存在", null);
            }
            else if (sUser.IsLocked)
            {
                return new OperationResult(OperationResultType.ValidError, "该账号已被冻结，请联系客服。", null);
            }
            else if (!await UserManager.CheckPasswordAsync(sUser, password))
            {
                return new OperationResult(OperationResultType.ValidError, "用户名或密码错误", null);
            }
            else
            {
                //更新最后一次登录的RegistKey
                var theUser = await UserInfos.SingleOrDefaultAsync(p => p.SysUser.UserName == userName);
                if (theUser == null)
                {
                    return new OperationResult(OperationResultType.ValidError, "数据错误", null);
                }

                if (theUser.RegistKey != registKey)
                {
                    theUser.RegistKey = registKey;
                    await UserInfoRepo.UpdateAsync(theUser);
                }

                //变更登录信息
                await ResetToken(theUser, loginDevice, clientVersion);

                var loginInfo = new UserTokenDto()
                {
                    Id = theUser.Id,
                    NickName = theUser.SysUser.NickName,
                    HeadPic = theUser.HeadPic,
                    Sex = theUser.Sex,
                    Token = theUser.Token
                };
                return new OperationResult(OperationResultType.Success, "登录成功", loginInfo);
            }
        }

        /// <summary>
        /// 更改用户名
        /// </summary>
        /// <param name="userName">原用户名</param>
        /// <param name="newUserName">新用户名</param>
        /// <param name="password">登录密码</param>
        /// <param name="validateCode">验证码</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> ChangeUserName(string userName, string newUserName,string password, string validateCode)
        {
            userName.CheckNotNullOrEmpty("userName");
            newUserName.CheckNotNullOrEmpty("newUserName");
            validateCode.CheckNotNullOrEmpty("validateCode");
            //验证码
            var severCode = GetValidateCode(userName, CodeType.更换手机);
            if (severCode == null || severCode.Code != validateCode)
            {
                return await Task.FromResult(new OperationResult(OperationResultType.ValidError, "验证码错误"));
            }

            var sUser = await UserManager.FindByNameAsync(userName);
            
            if (sUser == null)
            {
                return new OperationResult(OperationResultType.NoChanged, "用户不存在");
            }

            if (!await UserManager.CheckPasswordAsync(sUser, password))
            {
                return new OperationResult(OperationResultType.ValidError, "登录密码错误", null);
            }

            if (SysUserRepo.CheckExists(p => p.UserName == newUserName, sUser.Id))
            {
                return new OperationResult(OperationResultType.NoChanged, "该用户名已存在");
            }

            sUser.UserName = userName;
            if (severCode.ValidateType == ValidateType.手机)
            {
                sUser.PhoneNumber = newUserName;
                sUser.PhoneNumberConfirmed = true;
            }
            else
            {
                sUser.Email = newUserName;
                sUser.EmailConfirmed = true;
            }
            await SysUserRepo.UpdateAsync(sUser);
            return new OperationResult(OperationResultType.Success, "更改成功");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="oldPsw">原密码</param>
        /// <param name="newPsw">新密码</param>
        /// <returns></returns>
        public async Task<OperationResult> ChangePassword(string userName,string oldPsw,string newPsw)
        {
            userName.CheckNotNullOrEmpty("userName");
            oldPsw.CheckNotNullOrEmpty("oldPsw");
            newPsw.CheckNotNullOrEmpty("newPsw");

            var sUser = await UserManager.FindByNameAsync(userName);

            if (sUser == null)
            {
                return new OperationResult(OperationResultType.NoChanged, "用户不存在");
            }

            if (!await UserManager.CheckPasswordAsync(sUser, oldPsw))
            {
                return new OperationResult(OperationResultType.ValidError, "原密码错误", null);
            }
            UserManager.RemovePassword(sUser.Id);
            UserManager.AddPassword(sUser.Id, newPsw);
            return new OperationResult(OperationResultType.Success, "密码修改成功");
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="newPsw">新密码</param>
        /// <param name="validateCode">验证码</param>
        /// <returns></returns>
        public async Task<OperationResult> ResetPassword(string userName, string newPsw, string validateCode)
        {
            userName.CheckNotNullOrEmpty("userName");
            newPsw.CheckNotNullOrEmpty("newPsw");
            validateCode.CheckNotNullOrEmpty("validateCode");

            //验证码
            var severCode = GetValidateCode(userName, CodeType.找回密码);
            if (severCode == null || severCode.Code != validateCode)
            {
                return await Task.FromResult(new OperationResult(OperationResultType.ValidError, "验证码错误"));
            }

            var sUser = await UserManager.FindByNameAsync(userName);
            if (sUser == null)
            {
                return new OperationResult(OperationResultType.NoChanged, "用户不存在");
            }
            UserManager.RemovePassword(sUser.Id);
            UserManager.AddPassword(sUser.Id, newPsw);
            return new OperationResult(OperationResultType.Success, "密码重置成功");
        }

        /// <summary>
        /// 重置用户Token有效期
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="loginDevice">登录设备</param>
        /// <param name="clientVersion">客户端版本</param>
        /// <returns></returns>
        public async Task<OperationResult> ResetToken(UserInfo user, LoginDevice loginDevice,string clientVersion)
        {
            Operator oper = new Operator()
            {
                UserId = user.Id.ToString(),
                UserName = user.SysUser.UserName,
                LoginDevice = loginDevice,
                PhoneNo = user.SysUser.PhoneNumber,
                ClientVersion = clientVersion,
                ValidatePeriod = DateTime.Now.AddDays(30),//默认30天有效期
                UserDatas = new Dictionary<string, object>()
            };
            string strAuth = oper.ToJsonString();
            user.Token = DesHelper.Encrypt(strAuth, OSharp.Core.Constants.BodeAuthDesKey);

            await UserInfoRepo.UpdateAsync(user);
            return new OperationResult(OperationResultType.Success, "重置成功", user.Token);
        }

        #region 私有方法

        /// <summary>
        /// 从数据库读取验证码
        /// </summary>
        /// <param name="codeKey">验证码Key</param>
        /// <param name="codeType">验证码类型</param>
        /// <returns></returns>
        private ValidateCode GetValidateCode(string codeKey, CodeType codeType)
        {
            var dt = DateTime.Now.AddHours(-1);
            return ValidateCodes.Where(p => p.CodeKey == codeKey && p.CodeType == codeType && p.CreatedTime >= dt)
                    .OrderByDescending(p => p.CreatedTime).FirstOrDefault();
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="codeKey">验证码Key</param>
        /// <param name="validateType">验证方式</param>
        /// <param name="codeType">验证码类型</param>
        /// <param name="sendAction">发送委托</param>
        /// <returns>业务操作结果</returns>
        private async Task<OperationResult> SendValidateCode(string codeKey, ValidateType validateType, CodeType codeType, Action<string> sendAction)
        {
            codeKey.CheckNotNull("codeKey");
            sendAction.CheckNotNull("sendAction");

            string validateCode = new ValidateCoder().GetCode(6, ValidateCodeType.Number);
            var codeEntity = new ValidateCode()
            {
                CodeKey = codeKey,
                Code = validateCode,
                ValidateType = validateType,
                CodeType = codeType
            };
            await ValidateCodeRepo.InsertAsync(codeEntity);

            sendAction(validateCode);
            return new OperationResult(OperationResultType.Success, "验证码发送成功", "");
        }
        #endregion

    }
}
