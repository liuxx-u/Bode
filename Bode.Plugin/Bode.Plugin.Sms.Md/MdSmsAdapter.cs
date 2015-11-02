using Bode.Plugin.Core.SMS;

namespace Bode.Plugin.Sms.Md
{
    public class MdSmsAdapter : ISmsAdapter
    {
        private static readonly ISms Instance = new MdSms();

        /// <summary>
        ///获取短信发送执行者实例 
        /// </summary>
        public ISms SmsInstance
        {
            get { return Instance; }
        }
    }
}
