using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;

namespace OSharp.Web.Mvc.Upload
{
    public static class UpLoadHelper
    {
        private static readonly string UploadRoot = "~/UploadFile/";
        private static readonly string ServerHost = ConfigurationManager.AppSettings["ServerHost"];

        #region MVC

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="hfc">客户端上传文件流</param>
        /// <param name="afterPath">类似：aaaa/bbb/子文件夹</param>
        /// <returns></returns>
        public static List<string> MvcUpload(HttpFileCollectionBase hfc, string afterPath = "")
        {
            string path = UploadRoot + afterPath;

            List<string> theList = new List<string>();
            string filePath = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (string item in hfc)
            {
                if (hfc[item] == null || hfc[item].ContentLength == 0) continue;
                try
                {
                    string fileExtension = Path.GetExtension(hfc[item].FileName);
                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    hfc[item].SaveAs(filePath + fileName);

                    string strSqlPath = (path + fileName).Replace("~/", ServerHost);
                    theList.Add(strSqlPath);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex);
                }
            }
            return theList;
        }

        /// <summary>
        /// 以文件流的形式保存文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="suffix"></param>
        /// <param name="afterPath"></param>
        /// <returns></returns>
        public static string Upload(Stream stream, string suffix, string afterPath = "")
        {
            string path = UploadRoot + afterPath;

            if (stream.Length == 0) return string.Empty;

            string filePath = HttpContext.Current.Server.MapPath(path);
            string fileName = Guid.NewGuid().ToString() + "." + suffix;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            using (FileStream fs = new FileStream(filePath + fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);     //将流的内容读到缓冲区 
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
            return (path + fileName).Replace("~/", ServerHost); ;
        }

        #endregion

        #region WebApi

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="hfc">客户端上传文件流</param>
        /// <param name="afterPath">类似：aaaa/bbb/子文件夹</param>
        /// <returns></returns>
        public static List<string> ApiUpload(HttpFileCollection hfc, string afterPath = "")
        {
            string path = string.Format("{0}{1}{2}/", UploadRoot, afterPath, DateTime.Today.ToString("yyyyMMdd"));

            List<string> theList = new List<string>();

            string filePath = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (string itemKey in hfc)
            {
                HttpPostedFile item = hfc[itemKey];
                if (item != null && item.ContentLength == 0) continue;

                string fileExtension = Path.GetExtension(item.FileName);
                var fileName = Guid.NewGuid() + fileExtension;
                item.SaveAs(filePath + fileName);

                string strSqlPath = (path + fileName).Replace("~/", ServerHost);
                theList.Add(strSqlPath);
            }
            return theList;
        }



        #endregion
    }
}
