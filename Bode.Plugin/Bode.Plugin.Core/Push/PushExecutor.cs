using System;
using System.Collections.Generic;
using Bode.Plugin.Core.Properties;
using OSharp.Utility.Logging;

namespace Bode.Plugin.Core.Push
{
    public class PushExecutor:IPush
    {
        private readonly IPush _instance;
        private static readonly ILogger Logger = LogManager.GetLogger<PushExecutor>();


        public PushExecutor(string name)
        {
            IPushAdapter adapter = PushManager.PushAdapter;
            if (adapter == null)
            {
                Logger.Warn(Resources.Push_NotInitialized);
            }
            else
            {
                _instance = adapter.GetPushInstance(name);
            }
        }

        public PushExecutor(Type type) : this(type.FullName)
        {
        }

        /// <summary>
        /// 推送全部用户
        /// </summary>
        /// <returns>是否推送成功</returns>
        public bool PushAll(Dictionary<string, object> objs)
        {
            if (_instance != null)
            {
                return _instance.PushAll(objs);
            }
            return false;
        }
        
        /// <summary>
        /// 推送指定用户
        /// </summary>
        /// <param name="registId">注册Key</param>
        /// <param name="content">内容</param>
        /// <param name="objs">数据</param>
        /// <returns>是否推送成功</returns>
        public bool PushByRegistId(string registId, string content, Dictionary<string, object> objs)
        {
            if (_instance != null)
            {
                return _instance.PushByRegistId(registId, content, objs);
            }
            return false;
        }
    }
}
