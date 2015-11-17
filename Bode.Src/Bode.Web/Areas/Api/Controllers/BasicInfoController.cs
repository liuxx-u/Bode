using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using OSharp.Utility.Data;
using OSharp.Web.Http.Messages;
using OSharp.Web.Mvc.Upload;
using OSharp.Utility.Extensions;

namespace Bode.Web.Areas.Api.Controllers
{
    [Description("基础功能接口")]
    public class BasicInfoController : ApiController
    {
        [HttpPost]
        [Description("上传图片")]
        public IHttpActionResult UploadPic()
        {
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0) return Json(new { returnCode = "0", returnMsg = "没有文件" });

            var filePath = UpLoadHelper.ApiUpload(files, "Pic/").FirstOrDefault();

            if (filePath.IsNullOrWhiteSpace())
            {
                return Json(new ApiResult(OperationResultType.Error, "上传失败"));
            }
            return Json(new ApiResult("上传成功", filePath));
        }


        [HttpPost]
        [Description("批量上传图片")]
        public IHttpActionResult UploadPics()
        {
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0) return Json(new { returnCode = "0", returnMsg = "没有文件" });

            var filePaths = UpLoadHelper.ApiUpload(files, "Pic/");

            if (!filePaths.Any())
            {
                return Json(new ApiResult(OperationResultType.Error, "上传失败"));
            }
            return Json(new ApiResult("上传成功", filePaths));
        }
    }
}
