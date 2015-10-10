using OSharp.Utility.IM;
using System.Collections.Concurrent;

namespace Bode.Adapter.IM.Easemob
{
    public class EasemobImProvider : IImProvider
    {
        private static readonly ConcurrentDictionary<string, IIm> ims;

        static EasemobImProvider()
        {
            ims = new ConcurrentDictionary<string, IIm>();
        }

        /// <summary>
        /// 获取 即时聊天是否可用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 获取即时聊天对象
        /// </summary>
        /// <param name="regionName">区域名称</param>
        /// <returns></returns>
        public IIm GetImInstance(string regionName)
        {
            IIm instance;
            if (ims.TryGetValue(regionName, out instance))
            {
                return instance;
            }
            instance = new EasemobIm();
            ims[regionName] = instance;
            return instance;
        }
    }
}
