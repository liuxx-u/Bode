using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bode.Web.Areas.Admin.ViewModels
{
    public class ClassifyNode
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public int Order { get; set; }

        public IEnumerable<ClassifyNode> Children { get; set;}
    }
}