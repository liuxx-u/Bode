using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Plugin.Core.IM
{
    public interface IImAdapter
    {

        /// <summary>
        /// 获取IM执行实例
        /// </summary>
        /// <returns></returns>
        IIm ImInstance { get; }
    }
}
