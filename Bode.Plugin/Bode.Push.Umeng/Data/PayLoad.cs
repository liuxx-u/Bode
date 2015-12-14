using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Push.Umeng.Data
{
    public class PayLoad
    {
        public PayLoad()
        {
            display_type = DisplayType.notification;
            body = new Dictionary<string, object>();
        }

        // 必填 消息类型
        public DisplayType display_type { get; set; }
        
        // 必填 消息体。
        //display_type=message时,body的内容只需填写custom字段。display_type=notification时, body包含如下参数:
        public Dictionary<string, object> body { get; set; }

        #region 消息体注释

                /*
                // 通知展现内容:
                "ticker":"xx",     // 必填 通知栏提示文字
                "title":"xx",      // 必填 通知标题
                "text":"xx",       // 必填 通知文字描述 

                // 自定义通知图标:
                "icon":"xx",       // 可选 状态栏图标ID, R.drawable.[smallIcon],
                                           如果没有, 默认使用应用图标。
                                           图片要求为24*24dp的图标,或24*24px放在drawable-mdpi下。
                                           注意四周各留1个dp的空白像素
                "largeIcon":"xx",  // 可选 通知栏拉开后左侧图标ID, R.drawable.[largeIcon].
                                           图片要求为64*64dp的图标,
                                           可设计一张64*64px放在drawable-mdpi下,
                                           注意图片四周留空，不至于显示太拥挤
                "img": "xx",       // 可选 通知栏大图标的URL链接。该字段的优先级大于largeIcon。
                                           该字段要求以http或者https开头。

                // 自定义通知声音:
                "sound": "xx",     // 可选 通知声音，R.raw.[sound]. 
                                           如果该字段为空，采用SDK默认的声音, 即res/raw/下的
                                               umeng_push_notification_default_sound声音文件
                                           如果SDK默认声音文件不存在，
                                               则使用系统默认的Notification提示音。

                // 自定义通知样式:
                "builder_id": xx   // 可选 默认为0，用于标识该通知采用的样式。使用该参数时, 
                               开发者必须在SDK里面实现自定义通知栏样式。

                // 通知到达设备后的提醒方式
                "play_vibrate":"true/false", // 可选 收到通知是否震动,默认为"true".
                               注意，"true/false"为字符串
                "play_lights":"true/false",  // 可选 收到通知是否闪灯,默认为"true"
                "play_sound":"true/false",   // 可选 收到通知是否发出声音,默认为"true"

                // 点击"通知"的后续行为，默认为打开app。
                "after_open": "xx" // 必填 值可以为:
                                           "go_app": 打开应用
                                           "go_url": 跳转到URL
                                           "go_activity": 打开特定的activity
                                           "go_custom": 用户自定义内容。
                "url": "xx",       // 可选 当"after_open"为"go_url"时，必填。
                                           通知栏点击后跳转的URL，要求以http或者https开头  
                "activity":"xx",   // 可选 当"after_open"为"go_activity"时，必填。
                                           通知栏点击后打开的Activity
                "custom":"xx"/{}   // 可选 display_type=message, 或者
                                           display_type=notification且
                                           "after_open"为"go_custom"时，
                                           该字段必填。用户自定义内容, 可以为字符串或者JSON格式。
                */

                #endregion
    }
}
