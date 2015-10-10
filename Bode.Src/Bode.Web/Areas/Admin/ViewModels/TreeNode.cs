using System;
using System.Collections.Generic;


namespace Bode.Web.Areas.Admin.ViewModes
{
    /// <summary>
    /// 树节点
    /// </summary>
    public class TreeNode
    {
        public TreeNode()
        {
            Checked = false;
        }

        public Guid Id { get; set; }

        public string Text { get; set; }

        public bool Checked { get; set; }

        public int Order { get; set; }

        public string IconCls { get; set; }

        public string Url { get; set; }

        public ICollection<TreeNode> Children { get; set; }
    }
}