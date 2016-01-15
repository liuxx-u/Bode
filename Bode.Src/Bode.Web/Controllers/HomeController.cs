using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bode.Plugin.Core.SMS;
using Bode.Services.Core.Contracts;
using Bode.Services.Core.Dtos.User;
using Bode.Services.Core.Models.User;
using OSharp.Core.Data;
using OSharp.Utility.Develop.T4;
using OSharp.Utility.Extensions;
using OSharp.Utility.Secutiry;

namespace Bode.Web.Controllers
{
     [Description("系统主页")]
    public class HomeController : Controller
    {
        public IUserContract UserContract { get; set; }

         // GET: Home

         [Description("主页")]
        public async Task<ActionResult> Index()
        {
            //string modelFile = Path.Combine(projectPath, @"bin\Debug\Bode.Services.Core.dll");
            //byte[] fileData = File.ReadAllBytes(modelFile);
            //Assembly assembly = Assembly.Load("Bode.Services.Core");
            //Type baseType = typeof(EntityBase<>);
            //IEnumerable<Type> modelTypes = assembly.GetTypes().Where(m => baseType.IsGenericAssignableFrom(m) && !m.IsAbstract && m.HasAttribute<GenerateAttribute>());

             ValidateCodeDto validate = new ValidateCodeDto()
             {
                 CodeKey = "15884517874",
                 Code = "123456",
                 CodeType = CodeType.用户注册
             };

             //await UserContract.SaveValidateCodes(dtos: validate);

             var codes= await UserContract.ValidateCodes.ToListAsync();
             return Content("as");
        }
    }
}