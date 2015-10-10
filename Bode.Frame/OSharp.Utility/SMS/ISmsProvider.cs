
namespace OSharp.Utility.SMS
{
    public interface ISmsProvider
    {
        /// <summary>
        /// 获取 短信是否可用
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// 获取发送短信对象
        /// </summary>
        /// <returns></returns>
        ISms GetSmsInstance(string regionName);
    }
}
