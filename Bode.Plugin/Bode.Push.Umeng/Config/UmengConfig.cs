using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Push.Umeng.Config
{
    public class UmengConfig
    {
        //友盟App Key
        public static readonly string Appkey;

        //友盟App Master Secret
        public static readonly string AppSecret;

        public static readonly string AliasType;

        static UmengConfig()
        {
            Appkey = ConfigurationManager.AppSettings["PUSH_UMENG_APPKEY"];
            AppSecret= ConfigurationManager.AppSettings["PUSH_UMENG_APPSECRET"];
            AliasType = ConfigurationManager.AppSettings["PUSH_UMENG_ALIASTYPE"];
        }
    }
}
