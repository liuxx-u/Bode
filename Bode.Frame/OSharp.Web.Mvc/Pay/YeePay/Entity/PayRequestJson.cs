using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Web.Mvc.Pay.YeePay
{
    /// <summary>
    /// 支付请求易宝处理实体
    /// </summary>
    public class PayRequestJson
    {
        /// <summary>
        /// 商户编号
        /// </summary>
        public string customernumber { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string requestid { get; set; }

        /// <summary>
        /// 返回码成功返回：1
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 易宝交易流水号
        /// </summary>
        public string externalid { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>

        public string amount { get; set; }

        /// <summary>
        /// 支付链接
        /// </summary>
        public string payurl { get; set; }

        /// <summary>
        /// 绑卡 ID  
        /// </summary>
        public string bindid { get; set; }

        /// <summary>
        /// 银行编码
        /// </summary>
        public string bankcode { get; set; }

        /// <summary>
        /// 签名信息
        /// </summary>
        public string hmac { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string msg { get; set; }

    }
}
