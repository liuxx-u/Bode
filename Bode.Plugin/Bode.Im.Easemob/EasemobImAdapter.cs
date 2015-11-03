using Bode.Plugin.Core.IM;

namespace Bode.Im.Easemob
{
    public class EasemobImAdapter : IImAdapter
    {
        private static readonly IIm Instance = new EasemobIm();

        /// <summary>
        /// 获取IM执行实例
        /// </summary>
        public IIm ImInstance
        {
            get { return Instance; }
        }
    }
}
