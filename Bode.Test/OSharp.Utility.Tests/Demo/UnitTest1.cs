using System;
using System.Collections.Generic;
using System.Text;
using OSharp.Utility.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSharp.Utility.Secutiry;

namespace OSharp.Utility.Tests.Demo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<String, String> signPackage = new Dictionary<String, String>();

            signPackage.Add("orderNumber", "YD2015013010285194");
            signPackage.Add("hotelCode", "0731-2368888820140723043525");
            signPackage.Add("channel", "1");
            signPackage.Add("inDate", "2015-01-30 12:00:00");
            signPackage.Add("outDate", "2015-01-31 12:00:00");
            signPackage.Add("count", "1");
            signPackage.Add("price", "1.0");
            signPackage.Add("roomTypeCode", "商务双人床");
            signPackage.Add("name", "蒋林洋");
            signPackage.Add("phone", "18501790759");
            signPackage.Add("orderTime", "2015-01-30 10:28:51");

            string md5 = Md5(signPackage);

            string validate = HashHelper.GetMd5(md5, Encoding.UTF8);
        }

        private string Md5(Dictionary<String, String> dic)
        {
            StringBuilder sb = new StringBuilder();

            string orderNumber = dic["orderNumber"];
            string hotelCode = dic["hotelCode"];
            string channel = dic["channel"];
            string inDate = dic["inDate"];
            string outDate = dic["outDate"];
            string count = dic["count"];
            string price = dic["price"];
            string roomTypeCode = dic["roomTypeCode"];
            string name = dic["name"];
            string phone = dic["phone"];
            string orderTime = dic["orderTime"];

            string md5OrderNumber = HashHelper.GetMd5(orderNumber,Encoding.UTF8);
            string md5HotelCode = HashHelper.GetMd5(hotelCode, Encoding.UTF8);
            string md5Channel = HashHelper.GetMd5(channel, Encoding.UTF8);
            string md5InDate = HashHelper.GetMd5(inDate, Encoding.UTF8);
            string md5OutDate = HashHelper.GetMd5(outDate, Encoding.UTF8);
            string md5Count = HashHelper.GetMd5(count, Encoding.UTF8);
            string md5Price = HashHelper.GetMd5(price, Encoding.UTF8);
            string md5RoomTypeCode = HashHelper.GetMd5(roomTypeCode, Encoding.UTF8);
            string md5Name = HashHelper.GetMd5(name, Encoding.UTF8);
            string md5Phone = HashHelper.GetMd5(phone, Encoding.UTF8);
            string md5OrderTime = HashHelper.GetMd5(orderTime, Encoding.UTF8);


            sb.Append(md5OrderNumber);
            sb.Append(md5HotelCode);
            sb.Append(md5RoomTypeCode);
            sb.Append(md5Name);
            sb.Append(md5Phone);
            sb.Append(md5Channel);
            sb.Append(md5OrderTime);
            sb.Append(md5InDate);
            sb.Append(md5OutDate);
            sb.Append(md5Count);
            sb.Append(md5Price);
            return sb.ToString();
        }


        [TestMethod]
        public void PinYinTest()
        {
            string ch =
                "http://211.149.210.62/UploadFile/CarModelPic/阿斯顿·马丁/阿斯顿·马丁/阿斯顿·马丁DB9/2014款/ 6.0L Volante百年纪念版.jpg";

            string piny = StringHelper.GetChineseSpell(ch);
        }

        [TestMethod]
        public void ImageHelperTest()
        {
            var path = @"D:\Frame\Bode\Bode.Src\Bode.Web\Content\images\attach-blue.png";
            ImageHelper.GetThumbnail(path, 60);
        }
    }
}
