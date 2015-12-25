using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace OSharp.Web.Mvc.Pay.YeePay
{
    public class YeePayConfig
    {
        static YeePayConfig()
        {
            ////易宝支付分配的公钥，该公钥由商户进入商户后台先上报自己的公钥再获取，商户后台目录为（产品管理——RSA公钥管理）
            ////商户后台(测试环境http://mobiletest.yeepay.com/merchant,正式环境http://www.yeepay.com)
            //yibaoPublickey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCdaAde+egFkLwV7THPum4nPSBAJ2MGOaYBBldbKdbnCX8emCqXtp8OB9WIWbDVQfpNAH/s53Z/NW1pmjhLbbgOGcsEGd/feh/QIL80Wv26afqlLG/lTvUavnSdQs732/5viT+G/C9YWWp4MxqKTd8Va1b9BkzfpuvqcmAtiHkPBwIDAQAB";
            //商户账户编号
            merchantAccount = ConfigurationManager.AppSettings["YeePay-MerchantAccount"];
            merchantKey = ConfigurationManager.AppSettings["YeePay-MerchantKey"];
            AescKey = merchantKey.Substring(0, 16);
        }

        public static string merchantAccount
        { get; set; }

        public static string merchantKey { get; set; }

        public static string AescKey { get; set; }

    }
}
