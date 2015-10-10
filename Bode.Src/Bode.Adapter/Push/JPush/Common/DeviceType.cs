using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;


namespace Bode.Adapter.Push.Jpush.common
{
    public enum DeviceType
    {
        [Description("android")] android,
        [Description("ios")]     ios,
        [Description("winphone")]  winphone

    }
}
