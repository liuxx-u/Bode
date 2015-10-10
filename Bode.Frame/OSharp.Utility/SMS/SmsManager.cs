using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Utility.SMS
{
    public static class SmsManager
    {
        private static readonly object LockObj = new object();
        private static readonly ConcurrentDictionary<string, ISms> SmsInstances;

        static SmsManager() 
        {
            SmsInstances = new ConcurrentDictionary<string, ISms>();
        }

        /// <summary>
        /// 获取或设置 即时通讯提供程序
        /// </summary>
        private static ISmsProvider _provider;
        public static ISmsProvider Provider
        {
            get
            {
                return _provider;
            }
            set
            {
                lock (LockObj)
                {
                    _provider = value;
                }
            }
        }

        /// <summary>
        /// 获取指定区域的短信执行者实例
        /// </summary>
        public static ISms GetInstance(string region)
        {
            region.CheckNotNullOrEmpty("region");
            ISms instance;
            if (SmsInstances.TryGetValue(region, out instance))
            {
                return instance;
            }
            instance = new SmsExecutor(region);
            SmsInstances[region] = instance;
            return instance;
        }

        /// <summary>
        /// 获取指定类型的短信执行者实例
        /// </summary>
        public static ISms GetInstance(Type type)
        {
            type.CheckNotNull("type");
            return GetInstance(type.FullName);
        }
    }
}
