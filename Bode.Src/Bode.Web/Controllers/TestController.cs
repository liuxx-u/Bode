using Bode.Services.Core.Models.Identity;
using OSharp.Core.Data;
using OSharp.Core.Dependency;
using OSharp.Data.Entity;
using OSharp.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bode.Web.Controllers
{
    public class TestController : Controller
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IRepository<SysUser, int> UserRepo { get; set; }

        public ActionResult TestIoc()
        {
            const string format = "{0}: {1}";
            List<string> lines = new List<string>()
            {
                format.FormatWith("ServiceProvider", ServiceProvider.GetHashCode()),
                format.FormatWith("DefaultDbContext", ServiceProvider.GetService<DefaultDbContext>().GetHashCode()),
                format.FormatWith("DefaultDbContext", ServiceProvider.GetService<DefaultDbContext>().GetHashCode()),
                format.FormatWith("IRepository<User,int>", UserRepo.UnitOfWork.GetHashCode()),
                format.FormatWith("IRepository<User,int>", ServiceProvider.GetService<IRepository<SysUser,int>>().GetHashCode()),
            };
            return Content(lines.ExpandAndToString("<br>"));
        }
    }
}