using System.IO;

namespace Bode.Plugin.Core.SMS
{
    public class SmsManager
    {
        private static readonly object LockObj = new object();

        /// <summary>
        /// 获取或设置 IM适配器
        /// </summary>
        private static ISmsAdapter _smsAdapter;
        public static ISmsAdapter SmsAdapter
        {
            get { return _smsAdapter; }
            set
            {
                lock (LockObj)
                {
                    _smsAdapter = value;
                }
            }
        }

        /// <summary>
        /// 获取IM执行实例
        /// </summary>
        private static ISms _smsInstance;
        public static ISms SmsInstance
        {
            get { return _smsInstance ?? (_smsInstance = new SmsExecutor()); }
        }
    }
}
