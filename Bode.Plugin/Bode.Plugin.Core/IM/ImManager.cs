namespace Bode.Plugin.Core.IM
{
    public class ImManager
    {
        private static readonly object LockObj = new object();
        
        /// <summary>
        /// 获取或设置 IM适配器
        /// </summary>
        private static IImAdapter _imAdapter;
        public static IImAdapter ImAdapter
        {
            get { return _imAdapter; }
            set
            {
                lock (LockObj)
                {
                    _imAdapter = value;
                }
            }
        }

        /// <summary>
        /// 获取IM执行实例
        /// </summary>
        private static IIm _imInstance;
        public static IIm ImInstance
        {
            get { return _imInstance ?? (_imInstance = new ImExecutor()); }
        }
    }
}
