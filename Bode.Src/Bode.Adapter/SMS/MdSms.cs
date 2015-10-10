using OSharp.Utility;
using OSharp.Utility.SMS;
using OSharp.Utility.Extensions;
using OSharp.Utility.Secutiry;
using System.Configuration;
using System.Text;

namespace Bode.Adapter.SMS
{
    public class MdSms : SmsBase
    {
        private static md.sms.sdk.WebService sms = new md.sms.sdk.WebService();
        private static readonly string SN, PASSWORD;

        static MdSms()
        {
            SN = ConfigurationManager.AppSettings["MdSmsKey"];
            PASSWORD = ConfigurationManager.AppSettings["MdSmsPsw"];
        }

        public override bool Send(string phoneNo, string content, string sendTime)
        {
            phoneNo.CheckNotNullOrEmpty("phoneNo");
            content.CheckNotNullOrEmpty("content");
            if (!phoneNo.IsMobileNumber(true)) return false;

            //MD5加密密码
            string pwd = HashHelper.GetMd5(SN + PASSWORD, Encoding.UTF8);
            return sms.mt(SN, pwd, phoneNo, content, "", sendTime, "") == null;
        }
    }
}