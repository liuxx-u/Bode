using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Web.Mvc.Initialize
{
    /// <summary>
    /// 菜单分组属性(默认：area.controller)
    /// </summary>
    public class BodeMenuGroupKeyAttribute : Attribute
    {
        /// <summary>
        /// 分组键
        /// </summary>
        public string GroupKey { get; set; }

        public BodeMenuGroupKeyAttribute(string groupKey)
        {
            GroupKey = groupKey;
        }
    }
}
