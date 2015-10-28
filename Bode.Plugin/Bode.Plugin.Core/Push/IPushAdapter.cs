using System;

namespace Bode.Plugin.Core.Push
{
    public interface IPushAdapter
    {
        /// <summary>
        /// 由指定名称获取<see cref="IPush"/>推送实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        IPush GetPushInstance(string name);

        /// <summary>
        /// 由指定类型获取<see cref="IPush"/>推送实例
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        IPush GetPushInstance(Type type);
    }
}
