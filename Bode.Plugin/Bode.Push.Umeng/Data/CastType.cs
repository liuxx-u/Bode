using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Push.Umeng.Data
{
    public enum CastType
    {
        /// <summary>
        /// 单播
        /// </summary>
        unicast,

        /// <summary>
        /// 列播
        /// </summary>
        listcast,

        /// <summary>
        /// 广播
        /// </summary>
        broadcast,

        /// <summary>
        /// 组播
        /// </summary>
        groupcast,

        /// <summary>
        /// 自定义播
        /// </summary>
        customizedcast
    }
}
