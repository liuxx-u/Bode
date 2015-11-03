using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bode.Plugin.Core.SMS;
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

namespace Bode.Services.Implement.Services
{
    public partial class UserService
    {
        public ISms Sms { protected get; set; }

        /// <summary>
        /// 获取用户注册验证码
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <param name="codeType"></param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> GetSmsValidateCode(string phoneNo, CodeType codeType)
        {
            string validateCode = new ValidateCoder().GetCode(6, ValidateCodeType.Number);
            var codeEntity = new ValidateCode()
            {
                PhoneNo = phoneNo,
                Code = validateCode,
                CodeType = codeType
            };
            await ValidateCodeRepo.InsertAsync(codeEntity);

            //发送手机短信
            var smsContent = "您本次的验证码为" + validateCode + "，工作人员不会向您索要此验证码，请勿向任何人泄露。[右行养车]";
            Sms.Send(phoneNo,1, smsContent);


            return new OperationResult(OperationResultType.Success, "验证码发送成功", validateCode);
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
            var severCode = GetValidateCode(dto.PhoneNo, CodeType.用户注册);
            if (severCode != validateCode)
            {
                return new OperationResult(OperationResultType.ValidError, "验证码错误", 0);
            }

            if (UserInfoRepo.CheckExists(p => p.SysUser.PhoneNumber == dto.PhoneNo || p.SysUser.UserName == dto.PhoneNo))
            {
                return new OperationResult(OperationResultType.ValidError, "手机号已被使用", 0);
            }

            try
            {
                UserInfoRepo.UnitOfWork.TransactionEnabled = true;
                //验证密码格式
                IdentityResult result = await UserManager.PasswordValidator.ValidateAsync(dto.Password);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult();
                }

                SysUser sUser = new SysUser()
                {
                    UserName = dto.PhoneNo,
                    NickName = dto.NickName,
                    PhoneNumber = dto.PhoneNo,
                    PhoneNumberConfirmed = true,

                    //密码加密
                    PasswordHash = UserManager.PasswordHasher.HashPassword(dto.Password)
                };
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
        /// <param name="phoneNo">手机号</param>
        /// <param name="password">密码</param>
        /// <param name="registKey">极光registKey</param>
        /// <param name="loginDevice">登录设备</param>
        /// <param name="clientVersion">客户端版本</param>
        /// <returns></returns>
        public async Task<OperationResult> Login(string phoneNo, string password, string registKey, LoginDevice loginDevice, string clientVersion)
        {
            phoneNo.CheckNotNullOrEmpty("phoneNo");
            phoneNo.CheckNotNullOrEmpty("password");

            SysUser sUser = await UserManager.FindByNameAsync(phoneNo);
            var result = await UserManager.CheckPasswordAsync(sUser, password);

            if (!await UserManager.CheckPasswordAsync(sUser, password))
            {
                return new OperationResult(OperationResultType.ValidError, "用户名或密码错误", null);
            }
            else
            {
                //更新最后一次登录的RegistKey
                var theUser = await UserInfos.SingleOrDefaultAsync(p => p.SysUser.PhoneNumber == phoneNo);
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
                    Token = theUser.Token
                };
                return new OperationResult(OperationResultType.Success, "登录成功", loginInfo);
            }
        }

        /// <summary>
        /// 更改手机号
        /// </summary>
        /// <param name="phoneNo">原手机号</param>
        /// <param name="newPhoneNo">新手机号</param>
        /// <param name="password">登录密码</param>
        /// <param name="validateCode">验证码</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> ChangePhoneNo(string phoneNo, string newPhoneNo, string password, string validateCode)
        {
            phoneNo.CheckNotNullOrEmpty("phoneNo");
            newPhoneNo.CheckNotNullOrEmpty("newPhoneNo");
            validateCode.CheckNotNullOrEmpty("validateCode");
            //验证码
            var severCode = GetValidateCode(newPhoneNo, CodeType.更换手机);
            if (severCode != validateCode)
            {
                return await Task.FromResult(new OperationResult(OperationResultType.ValidError, "验证码错误"));
            }

            var sUser = await UserManager.FindByNameAsync(phoneNo);

            if (sUser == null)
            {
                return new OperationResult(OperationResultType.NoChanged, "用户不存在");
            }

            if (!await UserManager.CheckPasswordAsync(sUser, password))
            {
                return new OperationResult(OperationResultType.ValidError, "登录密码错误", null);
            }

            if (SysUserRepo.CheckExists(p => p.UserName == newPhoneNo || p.PhoneNumber == newPhoneNo, sUser.Id))
            {
                return new OperationResult(OperationResultType.NoChanged, "该手机号已注册");
            }

            sUser.UserName = newPhoneNo;
            sUser.PhoneNumber = newPhoneNo;
            await SysUserRepo.UpdateAsync(sUser);
            return new OperationResult(OperationResultType.Success, "手机号更改成功");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <param name="oldPsw">原密码</param>
        /// <param name="newPsw">新密码</param>
        /// <returns></returns>
        public async Task<OperationResult> ChangePassword(string phoneNo, string oldPsw, string newPsw)
        {
            phoneNo.CheckNotNullOrEmpty("phoneNo");
            oldPsw.CheckNotNullOrEmpty("oldPsw");
            newPsw.CheckNotNullOrEmpty("newPsw");

            var sUser = await UserManager.FindByNameAsync(phoneNo);

            if (sUser == null)
            {
                return new OperationResult(OperationResultType.NoChanged, "用户不存在");
            }

            if (!await UserManager.CheckPasswordAsync(sUser, oldPsw))
            {
                return new OperationResult(OperationResultType.ValidError, "原密码错误", null);
            }
            await UserStore.SetPasswordHashAsync(sUser, UserManager.PasswordHasher.HashPassword(newPsw));
            return new OperationResult(OperationResultType.Success, "密码修改成功");
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="phoneNo">电话号码</param>
        /// <param name="newPsw">新密码</param>
        /// <param name="validateCode">验证码</param>
        /// <returns></returns>
        public async Task<OperationResult> ResetPassword(string phoneNo, string newPsw, string validateCode)
        {
            phoneNo.CheckNotNullOrEmpty("phoneNo");
            newPsw.CheckNotNullOrEmpty("newPsw");
            validateCode.CheckNotNullOrEmpty("validateCode");

            //验证码
            var severCode = GetValidateCode(phoneNo, CodeType.找回密码);
            if (severCode != validateCode)
            {
                return await Task.FromResult(new OperationResult(OperationResultType.ValidError, "验证码错误"));
            }

            var sysUser = await UserManager.FindByNameAsync(phoneNo);
            if (sysUser == null)
            {
                return new OperationResult(OperationResultType.NoChanged, "用户不存在");
            }

            await UserStore.SetPasswordHashAsync(sysUser, UserManager.PasswordHasher.HashPassword(newPsw));


            //修改环信帐号密码
            //var imer = ImManager.GetInstance("resetPsw");
            //imer.ResetPsw<JObject>(phoneNo, newPsw);

            return new OperationResult(OperationResultType.Success, "密码重置成功");
        }

        /// <summary>
        /// 重置用户Token有效期
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="loginDevice">登录设备</param>
        /// <param name="clientVersion">客户端版本</param>
        /// <returns></returns>
        public async Task<OperationResult> ResetToken(UserInfo user, LoginDevice loginDevice, string clientVersion)
        {
            Operator oper = new Operator()
            {
                UserId = user.Id.ToString(),
                UserName = user.SysUser.UserName,
                LoginDevice = loginDevice,
                PhoneNo = user.SysUser.PhoneNumber,
                ClientVersion = clientVersion,
                ValidatePeriod = DateTime.Now.AddDays(30)//默认30天有效期
            };
            string authDesKey = "bodeauth";
            string strAuth = oper.ToJsonString();
            user.Token = DesHelper.Encrypt(strAuth, authDesKey);

            await UserInfoRepo.UpdateAsync(user);
            return new OperationResult(OperationResultType.Success, "重置成功");
        }

        #region 私有方法

        //获取验证码
        private string GetValidateCode(string phoneNo, CodeType codeType)
        {
            var dt = DateTime.Now.AddHours(-1);

            var codeEntity =
                ValidateCodes.Where(p => p.PhoneNo == phoneNo && p.CodeType == codeType && p.CreatedTime >= dt)
                    .OrderByDescending(p => p.CreatedTime).FirstOrDefault();

            return codeEntity == null ? string.Empty : codeEntity.Code;
        }
        #endregion

    }
}
