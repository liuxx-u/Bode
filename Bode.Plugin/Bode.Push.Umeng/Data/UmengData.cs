using Bode.Push.Umeng.Config;
using OSharp.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Push.Umeng.Data
{
    public class UmengData
    {
        public UmengData()
        {
            appkey = UmengConfig.Appkey;
            timestamp = DateTimeHelper.GetTimeStamp();
            type = CastType.customizedcast;
            alias_type = UmengConfig.AliasType;
            payload = new PayLoad();
            extra = new Dictionary<string, object>();
        }

        // 必填 应用唯一标识
        public string appkey { get; private set; }

        // 必填 时间戳，10位或者13位均可，时间戳有效期为10分钟
        public string timestamp { get; private set; }

        // 必填 消息发送类型
        public CastType type { get; set; }

        // 可选 设备唯一标识
        public string device_tokens { get; set; }

        // 可选 当type=customizedcast时，必填，alias的类型,
        public string alias_type { get; private set; }

        // 可选 当type=customizedcast时, 开发者填写自己的alias。
        public string alias { get; set; }

        // 必填 消息内容(Android最大为1840B)
        public PayLoad payload { get; set; }

        // 可选 用户自定义key-value。只对"通知"
        public Dictionary<string, object> extra { get; set; }

        // 可选 正式/测试模式。测试模式下，只会将消息发给测试设备。
        public string production_mode { get; set; }

        // 可选 发送消息描述，建议填写。
        public string description { get; set; }

        // 可选 开发者自定义消息标识ID, 开发者可以为同一批发送的多条消息提供同一个thirdparty_id, 便于友盟后台后期合并统计数据。 
        public string thirdparty_id { get; set; }

        public void SetAlias(string alias)
        {
            type = CastType.customizedcast;
            this.alias = alias;
        }
    }
}
