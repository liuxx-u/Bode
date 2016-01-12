using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System;

namespace OSharp.Utility.Helper
{
    /// <summary>  
    /// 图片处理类  
    /// 1、生成缩略图片或按照比例改变图片的大小和画质  
    /// 2、将生成的缩略图放到指定的目录下  
    /// </summary>  
    public static class ImageHelper
    {
        /// <summary>
        /// 缩略图回调
        /// </summary>
        private static Func<bool> ThumbnailCallback = delegate() { return false; };

        /// <summary>  
        /// 生成缩略图，返回缩略图的Image对象  
        /// </summary>  
        /// <param name="path">原图片全路径</param>  
        /// <param name="width">缩略图的宽度</param>  
        /// <param name="height">缩略图的高度</param>  
        /// <returns>缩略图的Image对象</returns>  
        public static Image GetThumbnail(string path,int width, int height)
        {
            try
            {
                Image resourceImage = Image.FromFile(path);//获取原图
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                return resourceImage.GetThumbnailImage(width, height, callb, IntPtr.Zero);
            }
            catch{return null;}
        }

        /// <summary>  
        /// 生成缩略图，将缩略图文件保存到指定的路径  
        /// </summary>  
        /// <param name="path">原图片全路径</param>  
        /// <param name="width">缩略图的宽度</param>  
        /// <param name="height">缩略图的高度</param>  
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:\Images\filename.jpg</param>  
        /// <returns>成功返回true，否则返回false</returns>  
        public static bool GetThumbnail(string path, int width, int height, string targetFilePath)
        {
            try
            {
                Image resourceImage = GetThumbnail(path, width, height);//获取原图
                resourceImage.Save(@targetFilePath, ImageFormat.Jpeg);
                //ReducedImage.Dispose();  
                return true;
            }
            catch { return false; }
        }

        /// <summary>  
        /// 生成缩略图，返回缩略图的Image对象  
        /// </summary>  
        /// <param name="path">原图片全路径</param> 
        /// <param name="percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>    
        /// <returns>缩略图的Image对象</returns>  
        public static Image GetThumbnail(string path,double percent)
        {
            try
            {
                Image resourceImage = Image.FromFile(path);//获取原图
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                int width = Convert.ToInt32(resourceImage.Width * percent);
                int height = Convert.ToInt32(resourceImage.Width * percent);
                return resourceImage.GetThumbnailImage(width, height, callb, IntPtr.Zero);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>  
        /// 生成缩略图，返回缩略图的Image对象  
        /// </summary>  
        /// <param name="path">原图片全路径</param> 
        /// <param name="percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>    
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:\Images\filename.jpg</param>  
        /// <returns>成功返回true,否则返回false</returns>  
        public static bool GetThumbnail(string path,double percent, string targetFilePath)
        {
            try
            {
                Image thumbnail = GetThumbnail(path, percent);
                thumbnail.Save(@targetFilePath, ImageFormat.Jpeg);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}