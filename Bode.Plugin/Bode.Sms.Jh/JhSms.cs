using Bode.Plugin.Core.SMS;
using Newtonsoft.Json.Linq;
using OSharp.Utility.Extensions;
using OSharp.Utility.Helper;
using System.Configuration;
using System.Text;

namespace Bode.Sms.Jh
{
    public class JhSms : ISms
    {
        private static readonly string Key;

        static JhSms()
        {
            Key = ConfigurationManager.AppSettings["SMS_JH_Key"];
        }
        public bool Send(string phoneNos, int templateId = 0, params string[] content)
        {
            return RetryHelper.Retry(() =>
            {
                var cont = "#code#=" + content.ExpandAndToString();
                var value = System.Web.HttpUtility.UrlEncode(cont, Encoding.UTF8);
                var url ="http://v.juhe.cn/sms/send?mobile=" + phoneNos + "&tpl_id=" + templateId + "&tpl_value=" + value + "&key=" + Key;
                var response = GetRequestHelper.GetWebRequest(url, RequestMethod.Get, encoding: Encoding.UTF8);
                var result= response.FromJsonString<JObject>();
                return result["reason"].ToString() == "发送成功";
            }, 4);
        }
    }
}
