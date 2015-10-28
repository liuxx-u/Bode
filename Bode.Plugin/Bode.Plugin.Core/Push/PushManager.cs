using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Utility;

namespace Bode.Plugin.Core.Push
{
    public class PushManager
    {
        public static readonly object LockObj = new object();

        public static readonly ConcurrentDictionary<string, IPush> Pushs;

        /// <summary>
        /// 获取或设置 IM适配器
        /// </summary>
        private static IPushAdapter _pushAdapter;
        public static IPushAdapter PushAdapter
        {
            get { return _pushAdapter; }
            set
            {
                lock (LockObj)
                {
                    _pushAdapter = value;
                }
            }
        }

        /// <summary>
        /// 获取推送者实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns>推送者实例</returns>
        public static IPush GetPushInstance(string name)
        {
            name.CheckNotNullOrEmpty("name");
            lock (LockObj)
            {
                IPush instance;
                if (Pushs.TryGetValue(name, out instance))
                {
                    return instance;
                }
                instance = new PushExecutor(name);
                Pushs[name] = instance;
                return instance;
            }
        }

        /// <summary>
        /// 获取推送者实例
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns>推送者实例</returns>
        public static IPush GetPushInstance(Type type)
        {
            type.CheckNotNull("type");
            return GetPushInstance(type.FullName);
        }

        /// <summary>
        /// 获取推送者实例
        /// </summary>
        /// <returns>推送者实例</returns>
        public static IPush GetPushInstance<T>()
        {
            return GetPushInstance(typeof(T));
        }
    }
}
