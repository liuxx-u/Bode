
namespace OSharp.Utility.IM
{
    public interface IImProvider
    {
        /// <summary>
        /// 获取 即时通讯是否可用
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// 获取即时通讯对象
        /// </summary>
        /// <returns></returns>
        IIm GetImInstance(string regionName);
    }
}
