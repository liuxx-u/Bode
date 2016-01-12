using Bode.Plugin.Core.Push;
using Bode.Push.Umeng.Config;
using Bode.Push.Umeng.Data;
using OSharp.Utility.Extensions;
using OSharp.Utility.Logging;
using OSharp.Utility.Secutiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Push.Umeng
{
    public class UmengPush : IPush
    {
        public ILogger _logger = LogManager.GetLogger<UmengPush>();

        #region IPush实现
        
        public bool PushAll(string tiker, string title, string text, Dictionary<string, object> objs)
        {
            UmengData data = GetPushData(tiker, tiker, text, objs);
            data.type = CastType.broadcast;

            return Push("http://msg.umeng.com/api/send", data);
        }
        
        public bool PushByKey(string pushKey, string tiker, string title, string text, Dictionary<string, object> objs)
        {
            UmengData data = GetPushData(tiker, tiker, text, objs);
            data.SetAlias(pushKey);

            return Push("http://msg.umeng.com/api/send", data);
        }

        #endregion

        #region 私有方法
        
        private UmengData GetPushData(string tiker, string title, string text, Dictionary<string, object> objs)
        {
            UmengData data = new UmengData();
            data.payload.SetTicker(tiker);
            data.payload.SetTitle(title);
            data.payload.SetText(text);
            data.extra = objs;
            return data;
        }

        private bool Push(string url, UmengData data)
        {
            string body = data.ToJsonString();
            var sign = GetSign(url, body);

            var pushUrl = string.Format("{0}?sign={1}", url, sign);

            WebClient client = new WebClient();
            var rep = client.UploadString(pushUrl, body);
            _logger.Info(rep);
            return true;
        }

        private string GetSign(string url, string body)
        {
            string method = "POST";
            string d = string.Format("${0}${1}${2}${3}", method, url, body, UmengConfig.AppSecret);
            return HashHelper.GetMd5(d, Encoding.UTF8);
        }

        public bool PushByAlias(List<string> alias, string tiker, string title, string text, Dictionary<string, object> objs)
        {
            throw new NotImplementedException();
        }

        public bool PushByTagWithinAnd(string tag, string tiker, string title, string text, Dictionary<string, object> objs)
        {
            throw new NotImplementedException();
        }

        public bool PushByTagWithinOr(string tag, string tiker, string title, string text, Dictionary<string, object> objs)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
