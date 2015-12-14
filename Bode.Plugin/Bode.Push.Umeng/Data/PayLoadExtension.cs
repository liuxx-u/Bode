using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Push.Umeng.Data
{
    public static class PayLoadExtension
    {
        /// <summary>
        /// 设置 通知栏提示文字
        /// </summary>
        /// <param name="payLoad">消息内容</param>
        /// <param name="tiker">提示文字</param>
        /// <returns></returns>
        public static PayLoad SetTicker(this PayLoad payLoad,string tiker)
        {
            payLoad.body.Add("ticker", tiker);
            return payLoad;
        }

        /// <summary>
        /// 设置 通知栏标题
        /// </summary>
        /// <param name="payLoad">消息内容</param>
        /// <param name="tiker">标题</param>
        /// <returns></returns>
        public static PayLoad SetTitle(this PayLoad payLoad, string title)
        {
            payLoad.body.Add("title", title);
            return payLoad;
        }

        /// <summary>
        /// 设置 文本内容
        /// </summary>
        /// <param name="payLoad">消息内容</param>
        /// <param name="tiker">文本内容</param>
        /// <returns></returns>
        public static PayLoad SetText(this PayLoad payLoad, string text)
        {
            payLoad.body.Add("text", text);
            return payLoad;
        }


    }
}
