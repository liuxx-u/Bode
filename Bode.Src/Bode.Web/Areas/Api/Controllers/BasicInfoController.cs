using System.ComponentModel;
using System.Linq;
using System.Web.Http;
using OSharp.Utility.Data;
using OSharp.Web.Http.Messages;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Drawing;
using OSharp.Utility.Helper;
using System.Configuration;
using System.Threading.Tasks;
using OSharp.Web.Http.Upload;
using OSharp.Core.Caching;
using System;

namespace Bode.Web.Areas.Api.Controllers
{
    [Description("基础功能接口")]
    public class BasicInfoController : ApiController
    {
        [HttpPost]
        [Description("上传图片")]
        public async Task<IHttpActionResult> UploadPic()
        {
            // 检查是否是 multipart/form-data
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Json(new ApiResult(OperationResultType.Error, "请求数据格式不正确"));
            }
            var paths = await ApiUploadHelper.Upload(Request.Content);
            if (!paths.Any()) return Json(new ApiResult(OperationResultType.NoChanged, "上传失败"));
            return Json(new ApiResult("上传成功", paths.First()));
        }

        [HttpPost]
        [Description("批量上传图片")]
        public async Task<IHttpActionResult> UploadPics()
        {
            // 检查是否是 multipart/form-data
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Json(new ApiResult(OperationResultType.Error, "请求数据格式不正确"));
            }
            var paths = await ApiUploadHelper.Upload(Request.Content);
            if (!paths.Any()) return Json(new ApiResult(OperationResultType.NoChanged, "上传失败"));
            return Json(new ApiResult("上传成功", paths));
        }

        [Description("获取指定尺寸的图片")]
        public HttpResponseMessage GetImage(string path, int width, int height)
        {
            ICache cache = CacheManager.GetCacher("img");
            string cacheKey = string.Format("{0}_{1}_{2}", path, width, height);
            MemoryStream ms = cache.Get<MemoryStream>(cacheKey);
            if (ms == null)
            {
                string host = ConfigurationManager.AppSettings["ServerHost"];
                path = path.Replace(host, "~/");
                path = System.Web.Hosting.HostingEnvironment.MapPath(path);
                Image img = ImageHelper.GetThumbnail(path, width, height);
                ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                cache.Set(cacheKey, ms, TimeSpan.FromMinutes(20));
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(ms.ToArray()); ;
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return result;
        }

        [Description("获取指定比例的图片")]
        public HttpResponseMessage GetRatioImage(string path, int percent = 100)
        {
            ICache cache = CacheManager.GetCacher("img");
            string cacheKey = string.Format("{0}_{1}", path, percent);
            MemoryStream ms = cache.Get<MemoryStream>(cacheKey);
            if (ms == null)
            {
                string host = ConfigurationManager.AppSettings["ServerHost"];
                path = path.Replace(host, "~/");
                path = System.Web.Hosting.HostingEnvironment.MapPath(path);
                Image img = ImageHelper.GetThumbnail(path, percent);
                ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                cache.Set(cacheKey, ms, TimeSpan.FromMinutes(20));
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(ms.ToArray()); ;
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return result;
        }
    }
}
