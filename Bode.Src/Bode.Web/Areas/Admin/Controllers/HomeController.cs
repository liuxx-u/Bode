using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Bode.Services.Core.Contracts;
using Bode.Web.Areas.Admin.ViewModes;
using OSharp.Core.Security;
using OSharp.Web.Mvc.Security;

namespace Bode.Web.Areas.Admin.Controllers
{
    [Description("管理主页")]
    public class HomeController : Controller
    {
        public ISecurityContract SecurityContract { get; set; }

        [Description("后台首页")]
        public ActionResult Index()
        {
            return View();
        }

        [AjaxOnly]
        [HttpPost]
        [Description("获取操作目录")]
        public ActionResult GetMenus()
        {
            var icons = new[]
            {
                "fa-th", "fa-desktop", "fa-table", "fa-bar-chart-o", "fa-pencil-square-o", "fa-picture-o", "fa-calendar",
                "fa-credit-card", "fa-hdd-o", "fa-tasks"
            };

            var functions =
                SecurityContract.Functions.Where(p => !p.IsLocked && !p.IsCustom && !p.IsAjax && p.PlatformToken == PlatformToken.Mvc)
                    .OrderBy(p => p.OrderNo)
                    .ToList();

            int i = 0;
            var menus = functions.Where(p => p.IsController).Select(p =>
            {
                return new TreeNode()
                {
                    Id = p.Id,
                    Text = p.Name,
                    IconCls = icons[i++ % icons.Count()],
                    Url = Url.Action(p.Action, p.Controller, new { area = p.Area }),
                    Children =
                        functions.Where(
                            m => m.Controller == p.Controller && m.Area == p.Area && !m.IsController && m.IsMenu)
                            .Select(m =>
                            {
                                string url = Url.Action(m.Action, m.Controller, new { area = m.Area });
                                if (url == "/") url = "";
                                return new TreeNode()
                                {
                                    Id = m.Id,
                                    Text = m.Name,
                                    IconCls = "",
                                    Url = url,
                                };
                            }).ToList()
                };
            }).ToList();
            return Json(menus);
        }

        [Description("后台欢迎页")]
        public ActionResult Welcome()
        {
            return View();
        }
    }
}