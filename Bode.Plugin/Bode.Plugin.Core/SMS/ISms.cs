namespace Bode.Plugin.Core.SMS
{
    public interface ISms
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNos">电话号码集合，逗号分隔</param>
        /// <param name="templateId">模版Id</param>
        /// <param name="content">短信内容</param>
        /// <returns>是否发送成功</returns>
        bool Send(string phoneNos, int templateId=0, params string[] content);
    }
}
