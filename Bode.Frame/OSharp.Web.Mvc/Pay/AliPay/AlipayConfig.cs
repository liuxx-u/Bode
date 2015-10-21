
namespace OSharp.Web.Mvc.Pay.AliPay
{
    /// <summary>
    /// 支付宝基础配置类，用于设置账户有关信息及返回路径
    /// </summary>
    public static class AlipayConfig
    {
        static AlipayConfig()
        {
            Partner = "2088021116482180";
            Key = "p8mykjguqa79v4esuog0cnzalbfttbdt";
            PublicKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
            InputCharset = "utf-8";
            SignType = "RSA";
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