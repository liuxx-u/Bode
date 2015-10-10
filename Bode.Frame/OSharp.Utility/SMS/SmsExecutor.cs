using System.Linq;
using OSharp.Utility.Logging;
using OSharp.Utility.Properties;
using System.Collections.Generic;

namespace OSharp.Utility.SMS
{
    //短信执行者
    internal class SmsExecutor : ISms
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(SmsExecutor));
        private readonly ISms instance;

        public SmsExecutor(string regionName) 
        {
            var provider = SmsManager.Provider;

            if (provider == null || !provider.Enabled)
            {
                Logger.Warn(Resources.SMS_NotInitialized);
            }
            else 
            {
                instance = provider.GetSmsInstance(regionName);
            }
        }

        /// <summary>
        /// 发送短信(只会发送成功一次)
        /// </summary>
        /// <param name="phoneNo">电话号码</param>
        /// <param name="content">短信内容</param>
        /// <param name="sendTime">发送时间</param>
        /// <returns></returns>
        public bool Send(string phoneNo, string content, string sendTime)
        {
            if (instance == null) return false;
            return instance.Send(phoneNo, content, sendTime);
        }
    }
}
