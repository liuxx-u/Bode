namespace Bode.Plugin.Core.SMS
{
    public interface ISmsAdapter
    {
        /// <summary>
        /// 获取SMS执行者实例
        /// </summary>
        ISms SmsInstance { get; }
    }
}
