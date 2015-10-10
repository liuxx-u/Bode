
namespace OSharp.Utility.SMS
{
    public abstract class SmsBase : ISms
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNo">电话号码</param>
        /// <param name="content">短信内容</param>
        /// <param name="sendTime">发送时间</param>
        /// <returns></returns>
        public abstract bool Send(string phoneNo, string content, string sendTime);
    }
}
