using Bode.Plugin.Core.Properties;
using OSharp.Utility.Logging;

namespace Bode.Plugin.Core.SMS
{
    public class SmsExecutor : ISms
    {
        private readonly ISms _instance;
        private static readonly ILogger Logger = LogManager.GetLogger<SmsExecutor>();

        public SmsExecutor()
        {
            ISmsAdapter adapter = SmsManager.SmsAdapter;
            if (adapter == null)
            {
                Logger.Warn(Resources.SMS_NotInitialized);
            }
            else
            {
                _instance = adapter.SmsInstance;
            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNos">电话号码集合，逗号分隔</param>
        /// <param name="templateId">模版Id</param>
        /// <param name="content">短信内容</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string phoneNos, int templateId = 0, params string[] content)
        {
            if (_instance != null)
            {
                return _instance.Send(phoneNos, templateId, content);
            }
            return false;
        }
    }
}
