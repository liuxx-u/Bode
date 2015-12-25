using OSharp.Utility.Logging;

namespace OSharp.Web.Mvc.Pay.WxPay
{
    public class Log
    {
        public static ILogger logger = LogManager.GetLogger("Wxpay");

        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content)
        {
            logger.Debug("{0}:{1}", className, content);
        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content)
        {
            logger.Info("{0}:{1}", className, content);
        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content)
        {
            logger.Error("{0}:{1}", className, content);
        }
    }
}