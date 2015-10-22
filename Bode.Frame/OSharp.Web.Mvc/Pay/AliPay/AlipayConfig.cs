
using System.Configuration;

namespace OSharp.Web.Mvc.Pay.AliPay
{
    /// <summary>
    /// 支付宝基础配置类，用于设置账户有关信息及返回路径
    /// </summary>
    public static class AlipayConfig
    {
        static AlipayConfig()
        {
            Partner = ConfigurationManager.AppSettings["AliPay-Partner"];
            Key = ConfigurationManager.AppSettings["AliPay-SecurityKey"];
            PublicKey = ConfigurationManager.AppSettings["AliPay-PublicKey"];
            InputCharset = ConfigurationManager.AppSettings["AliPay-InputCharset"] ?? "utf-8";
            SignType = ConfigurationManager.AppSettings["AliPay-SignType"] ?? "RSA";
        }

        /// <summary>
        /// 获取或设置 合作者身份ID
        /// </summary>
        public static string Partner { get; set; }

        /// <summary>
        /// 获取或设置 安全校验码
        /// </summary>
        public static string Key { get; set; }

        /// <summary>
        /// 获取或设置 字符编码格式，当前支持 gbk 或 utf-8
        /// </summary>
        public static string InputCharset { get; set; }

        /// <summary>
        /// 获取或设置 签名方式，当前支持 RSA、DSA、MD5
        /// </summary>
        public static string SignType { get; set; }

        /// <summary>
        /// 获取或设置 支付宝的公钥(无需修改该值)
        /// </summary>
        public static string PublicKey { get;set;}
    }
}