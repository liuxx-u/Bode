using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

using OSharp.Utility.Secutiry;


namespace OSharp.Web.Mvc.Pay.AliPay
{
    /// <summary>
    /// 支付宝通知处理类，处理支付宝各接口通知返回
    /// </summary>
    public class AlipayNotify
    {
        #region 字段

        private const string HttpsVerifyUrl = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
        private readonly string _inputCharset; //字符编码格式
        private readonly string _key; //商户的私钥
        private readonly string _publicKey;//支付宝公钥
        private readonly string _partner; //合作身份者ID
        private readonly string _signType; //签名方式
        private readonly SortedDictionary<string, string> _paras;

        //支付宝消息验证地址

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化一个 支付宝通知处理类 的新实例
        /// </summary>
        /// <param name="paras">通知返回参数的有序字典</param>
        public AlipayNotify(SortedDictionary<string, string> paras)
        {
            _partner = AlipayConfig.Partner.Trim();
            _key = AlipayConfig.Key.Trim();
            _publicKey = AlipayConfig.PublicKey.Trim();
            _inputCharset = AlipayConfig.InputCharset.Trim().ToLower();
            _signType = AlipayConfig.SignType.Trim().ToUpper();
            _paras = paras;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取 支付宝通知信息
        /// </summary>
        public AlipayNotifyInfo NotifyInfo { get; private set; }

        #endregion

        /// <summary>
        /// 验证消息是否是支付宝发出的合法消息
        /// </summary>
        /// <param name="notifyId">通知验证ID</param>
        /// <param name="sign">支付宝生成的签名结果</param>
        /// <returns>验证结果</returns>
        public bool Verify(string notifyId, string sign)
        {
            //获取返回时的签名验证结果
            bool isSign = GetSignVerify(sign);
            //获取是否是支付宝服务器发来的请求的验证结果
            string responseText = "true";
            if (!string.IsNullOrEmpty(notifyId))
            {
                responseText = GetResponseText(notifyId);
            }

            //写日志记录（若要调试，请取消下面两行注释）
            //string word = "responseTxt=" + responseText + "\n isSign=" + isSign + "\n 返回回来的参数：" + GetPreSignStr() + "\n ";
            //AlipayCore.LogResult(word);

            //判断responseText是否为true，isSign是否为true
            //responseText的结果不是true，与服务器设置问题、合作身份者ID，notify_id一分钟失效有关
            //isSign不是true，与安全校验码、请求时的参数格式（如：带自定义参数等）、编码格式有关
            bool isVerify = responseText == "true" && isSign;
            if (isVerify)
            {
                NotifyInfo = new AlipayNotifyInfo(_paras);
            }
            return isVerify;
        }

        /// <summary>
        /// 获取待签名字符串（调试用）
        /// </summary>
        /// <returns>待签名字符串</returns>
        public string GetPreSignStr()
        {
            //过滤空值、sign与signType参数
            Dictionary<string, string> dictPara = AlipayCore.FilterPara(_paras);
            //获取待签名字符串
            string preSignStr = AlipayCore.CreateLinkString(dictPara);
            return preSignStr;
        }

        /// <summary>
        /// 获取返回时的签名验证结果
        /// </summary>
        /// <param name="sign">对比签名结果的方式</param>
        /// <returns>签名验证结果</returns>
        private bool GetSignVerify(string sign)
        {
            //过滤空值、sign与signType参数
            Dictionary<string, string> dictPara = AlipayCore.FilterPara(_paras);
            //获取待签名字符串
            string perSignStr = AlipayCore.CreateLinkString(dictPara);
            bool isSign = false;
            if (!string.IsNullOrEmpty(sign))
            {
                switch (_signType)
                {
                    case "MD5":
                        isSign = Md5Verify(perSignStr, sign, _key, _inputCharset);
                        break;
                    case "RSA":
                        isSign = RsaVerify(perSignStr, sign, _publicKey, _inputCharset);
                        break;
                }
            }
            return isSign;
        }

        /// <summary>
        /// 获取是否是支付宝服务器发来的请求的验证结果
        /// </summary>
        /// <param name="notifyId">通知验证ID</param>
        /// <returns>验证结果</returns>
        private string GetResponseText(string notifyId)
        {
            string veryfyUrl = HttpsVerifyUrl + "partner=" + _partner + "&notify_id=" + notifyId;

            //获取远程服务器ATN结果，验证是否是支付宝服务器发来的请求
            string responseTxt = GetHttp(veryfyUrl, 120000);

            return responseTxt;
        }

        /// <summary>
        /// 获取远程服务器ATN结果
        /// </summary>
        /// <param name="strUrl">指定URL路径地址</param>
        /// <param name="timeout">超时时间设置</param>
        /// <returns>服务器ATN结果</returns>
        private string GetHttp(string strUrl, int timeout)
        {
            string strResult = string.Empty;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse httpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = httpWResp.GetResponseStream();
                if (myStream != null)
                {
                    StreamReader sr = new StreamReader(myStream, Encoding.Default);
                    StringBuilder strBuilder = new StringBuilder();
                    while (-1 != sr.Peek())
                    {
                        strBuilder.Append(sr.ReadLine());
                    }

                    strResult = strBuilder.ToString();
                }
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
            }

            return strResult;
        }

        private static bool Md5Verify(string prestr, string sign, string key, string inputCharset)
        {
            string info = string.Format("prestr:{0};sign:{1};key:{2};inputCharset:{3}", prestr, sign, key, inputCharset);
            AlipayCore.LogResult(info);

            Encoding encoding = Encoding.GetEncoding(inputCharset);
            string mysign = HashHelper.GetMd5(prestr + key, encoding);
            return mysign == sign;
        }

        private static bool RsaVerify(string content, string signedString, string publicKey, string inputCharset)
        {
            bool result = false;

            Encoding code = Encoding.GetEncoding(inputCharset);
            byte[] Data = code.GetBytes(content);
            byte[] data = Convert.FromBase64String(signedString);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);

            SHA1 sh = new SHA1CryptoServiceProvider();
            result = rsaPub.VerifyData(Data, sh, data);
            return result;
        }

        private static RSAParameters ConvertFromPublicKey(string pemFileConent)
        {

            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }
    }
}
