using Bode.Plugin.Core.Push;
using ifunction.JPush.V3;
using ifunction.JPush;
using OSharp.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Push.Jpush
{
    public class JPush : IPush
    {
        private static readonly ILogger Logger = LogManager.GetLogger("JPush");
        private readonly JPushClientV3 _pushClient;

        public JPush()
        {
            var appKey = ConfigurationManager.AppSettings["JPush_Key"].Trim();
            var masterSecret = ConfigurationManager.AppSettings["JPush_MasterSecret"].Trim();
            _pushClient = new JPushClientV3(appKey, masterSecret);
        }

        /// <summary>
        /// 推送全部用户
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>是否推送成功</returns>
        public bool PushAll(string title, string content,string text, Dictionary<string, object > objs)
        {
            return DoPost(CreatePushMsg(title, content, objs));
        }

        /// <summary>
        /// 推送指定用户
        /// </summary>
        /// <param name="registId">注册Key</param>
        /// <param name="content">内容</param>
        /// <param name="objs">自定义消息类型</param>
        /// <returns>是否推送成功</returns>
        public bool PushByKey(string pushKey, string tiker, string title, string text, Dictionary<string, object> objs)
        {
            return DoPost(CreatePushMsg("", tiker, objs, pushKey));
        }

        /// <summary>
        /// 创建消息实体
        /// </summary>
        /// <param name="title">消息标题</param>
        /// <param name="content">内容</param>
        /// <returns>消息实体</returns>
        private PushMessageRequestV3 CreatePushMsg(string title, string content, Dictionary<string, object> objs, string registId = null)
        {
            var customzedValues = new Dictionary<string, string>();
            if (objs != null)
            {
                foreach (var obj in objs)
                {
                    customzedValues.Add(obj.Key, obj.Value.ToString());
                }
            }
            Audience audience = new Audience();
            if (!string.IsNullOrEmpty(registId))
            {
                var registIdList = new List<string>();
                registIdList.Add(registId);
                audience.Add(PushTypeV3.ByRegistrationId, registIdList);
            }
            else
            {
                audience.Add(PushTypeV3.Broadcast, null);
            }
            //audience.Add(PushTypeV3.ByTagWithinAnd, new List<string>(new string[] { "Tag1", "Tag2" }));
            //audience.Add(PushTypeV3.ByTagWithinOr, new List<string>(new string[] { "Tag3", "Tag4" }));

            // In JPush V3, Notification would not be display on screen, it would be transferred to app instead.
            // And different platform can provide different notification data.
            Notification notification = new Notification
            {
                AndroidNotification = new AndroidNotificationParameters
                {
                    Title = title,
                    Alert = content,
                    CustomizedValues = customzedValues,
                },
                iOSNotification = new iOSNotificationParameters
                {
                    Badge = "+1",
                    Alert = content,
                    Sound = "YourSound",
                    CustomizedValues = customzedValues,
                }
            };

            PushMessageRequestV3 newPush = new PushMessageRequestV3
            {
                Audience = audience,
                IsTestEnvironment = true,
                AppMessage = new AppMessage
                {
                    Content = content
                },
                Notification = notification
            };

            return newPush;
        }

        /// <summary>
        /// 执行发送消息方法
        /// </summary>
        /// <param name="pushMsg">消息实体</param>
        /// <returns>是否推送成功</returns>
        private bool DoPost(PushMessageRequestV3 pushMsg)
        {
            var response = _pushClient.SendPushMessage(pushMsg);
            Logger.Info(response.ResponseCode.ToString() + ":" + response.ResponseMessage);
            return true;
        }
    }
}
