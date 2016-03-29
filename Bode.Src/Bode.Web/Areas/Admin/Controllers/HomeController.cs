using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Bode.Services.Core.Contracts;
using Bode.Web.Areas.Admin.ViewModes;
using OSharp.Core.Security;
using OSharp.Web.Mvc.Security;
using System.Threading.Tasks;
using System.Web.Security;
using OSharp.Utility.Data;
using OSharp.Web.Mvc.UI;
using System;
using OSharp.Utility.Helper;
using OSharp.Core.Data;
using OSharp.Web.Mvc.Initialize;

namespace Bode.Web.Areas.Admin.Controllers
{
    [Description("管理主页")]
    [BodeMenuGroupKey("Admin.Home")]
    public class HomeController : Controller
    {
        public ISecurityContract SecurityContract { get; set; }
        public IIdentityContract IdentityContract { get; set; }

        [Authorize]
        [BodeMenuGroupKey("mk222")]
        [Description("后台首页")]
        public ActionResult Index()
        {
            var user = IdentityContract.Users.SingleOrDefault(p => p.UserName == User.Identity.Name);
            ViewBag.User = user;
            return View();
        }

        [AjaxOnly]
        [HttpPost]
        [Authorize]
        [Description("获取操作目录")]
        public ActionResult GetMenus()
        {
            var icons = new[]
            {
                "fa-th", "fa-desktop", "fa-table", "fa-bar-chart-o", "fa-pencil-square-o", "fa-picture-o", "fa-calendar",
                "fa-credit-card","fa-laptop", "fa-hdd-o", "fa-tasks"," fa-list-alt"
            };

            //var userRoleIds = IdentityContract.UserRoleMaps
            //    .Where(p => p.User.UserName == User.Identity.Name).Select(p => p.Role.Id).Distinct().ToList();

            //var functions = SecurityContract.FunctionRoleMaps
            //    .Where(p => userRoleIds.Contains(p.Role.Id))
            //    .Where(p => !p.Function.IsLocked && !p.Function.IsCustom && !p.Function.IsAjax && p.Function.PlatformToken == PlatformToken.Mvc && p.Function.Controller != "Home")
            //    .Select(p => p.Function).Distinct().OrderBy(p => p.OrderNo).ToList();
            var functions =
                SecurityContract.Functions.Where(p => !p.IsLocked && !p.IsCustom && !p.IsAjax && p.PlatformToken == PlatformToken.Mvc && p.Controller != "Home")
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
                            m => m.MenuGroupKey==p.MenuGroupKey && !m.IsController && m.IsMenu)
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

        [Authorize]
        [Description("后台欢迎页")]
        public ActionResult Welcome()
        {
            return View();
        }

        [Authorize]
        [Description("退出登录")]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [AjaxOnly]
        [HttpPost]
        [Authorize]
        [Description("修改密码")]
        public async Task<ActionResult> UpdatePassword(string oldPassword, string newPassword)
        {
            var result = await IdentityContract.ResetPassword(User.Identity.Name, oldPassword, newPassword);
            return Json(result.ToAjaxResult());
        }

        [Description("登录页")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Description("登录提交")]
        public async Task<ActionResult> LoginPostDo(string userName, string password)
        {
            var result = await IdentityContract.Login(userName, password);
            if (result.ResultType == OperationResultType.Success)
            {
                FormsAuthentication.SetAuthCookie(userName, false);
            }
            return Json(result.ToAjaxResult());
        }
    }
}