using System.Collections.Generic;

namespace Bode.Plugin.Core.Push
{
    public interface IPush
    {
        /// <summary>
        /// 推送全部用户
        /// </summary>
        /// <returns>是否推送成功</returns>
        bool PushAll(Dictionary<string,object> objs);

        /// <summary>
        /// 推送指定用户
        /// </summary>
        /// <param name="registId">注册Key</param>
        /// <param name="content">内容</param>
        /// <param name="objs">数据</param>
        /// <returns>是否推送成功</returns>
        bool PushByRegistId(string registId, string content, Dictionary<string, object> objs);
    }
}
