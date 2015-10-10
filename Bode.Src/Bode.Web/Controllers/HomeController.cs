using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Bode.Services.Core.Contracts;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;
using OSharp.Utility.Extensions;

namespace Bode.Web.Controllers
{
     [Description("系统主页")]
    public class HomeController : Controller
    {
        public IUserContract UserContract { get; set; }

        // GET: Home

         [Description("主页")]
        public ActionResult Index()
        {
            //string modelFile = Path.Combine(projectPath, @"bin\Debug\Bode.Services.Core.dll");
            //byte[] fileData = File.ReadAllBytes(modelFile);
            Assembly assembly = Assembly.Load("Bode.Services.Core");
            Type baseType = typeof(EntityBase<>);
            IEnumerable<Type> modelTypes = assembly.GetTypes().Where(m => baseType.IsGenericAssignableFrom(m) && !m.IsAbstract && m.HasAttribute<GenerateAttribute>());
            return Content("as");
        }
    }
}