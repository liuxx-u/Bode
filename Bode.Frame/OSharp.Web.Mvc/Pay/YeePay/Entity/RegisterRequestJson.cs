using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Web.Mvc.Pay.YeePay
{
    public class RegisterRequestJson
    {
        /// <summary>
        /// 商户编号
        /// </summary>
        public string customernumber { get; set; }

        /// <summary>
        /// 请求注册号
        /// </summary>
        public string requestid { get; set; }

        /// <summary>
        /// 返回码成功返回：1
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 子账户编号
        /// </summary>
        public string ledgerno { get; set; }

        /// <summary>
        /// 签名信息
        /// </summary>
        public string hmac { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }
    }
}
