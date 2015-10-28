using Bode.Plugin.Core.SMS;

namespace Bode.Plugin.Sms.Ccp
{
    public class CcpSmsAdapter:ISmsAdapter
    {
        private static readonly ISms Instance = new CcpSms();

        /// <summary>
        ///获取短信发送执行者实例 
        /// </summary>
        public ISms SmsInstance 
        {
            get { return Instance; }
        }
    }
}
