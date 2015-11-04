﻿using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Bode.Plugin.Core.SMS;
using Bode.Sms.Md.sms.md.sdk;
using OSharp.Utility;
using OSharp.Utility.Extensions;
using OSharp.Utility.Helper;

namespace Bode.Sms.Md
{
    public class MdSms:ISms
    {
        private static readonly WebService sms = new WebService();
        private static readonly string Sn, Password;

        static MdSms()
        {
            Sn = ConfigurationManager.AppSettings["SMS_MD_Key"];
            Password = ConfigurationManager.AppSettings["SMS_MD_Psw"];
        }

        /// <summary>
        /// 发送短信(重试3次)
        /// </summary>
        /// <param name="phoneNos">电话号码集合，逗号分隔</param>
        /// <param name="templateId">模版Id</param>
        /// <param name="content">短信内容</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string phoneNos, int templateId = 0, params string[] content)
        {

            phoneNos.CheckNotNullOrEmpty("phoneNo");
            content.CheckNotNullOrEmpty("content");

            string strContent = content.ExpandAndToString();

            //MD5加密密码
            string pwd = GetMd5(Sn + Password);

            return RetryHelper.Retry(() =>
            {
                var result = sms.mt(Sn, pwd, phoneNos, strContent, "", "", "");
                return !result.IsNullOrWhiteSpace() && !result.StartsWith("-");
            }, 4);
        }


        #region 私有方法

        /// <summary>
        /// 密码加密
        /// </summary>
        private string GetMd5(string source)
        {
            try
            {
                MD5 getmd5 = new MD5CryptoServiceProvider();
                byte[] targetStr = getmd5.ComputeHash(Encoding.UTF8.GetBytes(source));
                var result = BitConverter.ToString(targetStr).Replace("-", "");
                return result;
            }
            catch (Exception)
            {
                return "0";
            }

        }

        #endregion
    }
}
