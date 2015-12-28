using Bode.Services.Core.Models.Identity;
using OSharp.Core.Data;
using OSharp.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bode.Web.Areas.Api.Controllers
{
    public class TestController : ApiController
    {
        public IRepository<SysUser, int> UserRepo { get; set; }
        public IRepository<SysRole, int> RoleRepo { get; set; }

        public IHttpActionResult test()
        {
            const string format = "{0}: {1}";
            List<string> lines = new List<string>()
            {
                format.FormatWith("IRepository<User,int>", UserRepo.UnitOfWork.GetHashCode()),
                format.FormatWith("IRepository<Role,int>", RoleRepo.UnitOfWork.GetHashCode()),
            };
            return Ok(lines.ExpandAndToString("<br>"));
        }
    }
}
