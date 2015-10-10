using System;
using System.Collections.Concurrent;

namespace OSharp.Utility.IM
{
    public class ImManager
    {
        private static readonly object LockObj = new object();
        private static ConcurrentDictionary<string, IIm> ImInstances;

        static ImManager() 
        {
            ImInstances = new ConcurrentDictionary<string, IIm>();
        }

        /// <summary>
        /// 获取或设置 即时通讯提供程序
        /// </summary>
        private static IImProvider _provider;
        public static IImProvider Provider 
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
        /// 获取指定区域的即时聊天执行者实例
        /// </summary>
        public static IIm GetInstance(string region)
        {
            region.CheckNotNullOrEmpty("region");
            IIm instance;
            if (ImInstances.TryGetValue(region, out instance))
            {
                return instance;
            }
            instance = new ImExecutor(region);
            ImInstances[region] = instance;
            return instance;
        }

        /// <summary>
        /// 获取指定类型的即时聊天执行者实例
        /// </summary>
        public static IIm GetInstance(Type type)
        {
            type.CheckNotNull("type");
            return GetInstance(type.FullName);
        }
    }
}
