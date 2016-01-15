using Bode.Services.Core.Contracts;
//using Bode.Services.Core.Models.Order;
using OSharp.Utility.Data;
using OSharp.Utility.Logging;
using OSharp.Web.Mvc;
using OSharp.Web.Mvc.Pay.AliPay;
using OSharp.Web.Mvc.Pay.WxPay;
using OSharp.Web.Mvc.Pay.YeePay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bode.Web.Areas.Admin.Controllers
{
    [Description("支付")]
    public class PayController : BaseController
    {
        //public IOrderContract OrderContract { get; set; }

        #region 支付回调

        [HttpPost]
        [Description("支付宝回调地址")]
        public async Task<ActionResult> AlipayNotifyUrl()
        {
            var loger = LogManager.GetLogger("Alipay");

            SortedDictionary<string, string> sPara = GetRequestPost();
            //loger.Error("params:{0}", Request.Params.ToJsonString());
            bool verifyResult = new AlipayNotify(sPara).Verify(Request.Form["notify_id"], Request.Form["sign"]);

            if (sPara.Count > 0 && verifyResult)
            {
                //商户订单号
                string outTradeNo = Request.Form["out_trade_no"];
                //支付宝交易号
                string tradeNo = Request.Form["trade_no"];
                //交易状态
                string tradeStatus = Request.Form["trade_status"];

                //打日志
                //loger.Error("orderNo:{0};tradeStatus:{1};", outTradeNo, tradeStatus);
                if (tradeStatus == "TRADE_FINISHED" || tradeStatus == "TRADE_SUCCESS")
                {
                    //await OrderContract.PayOrder(outTradeNo, PayType.支付宝);
                    return Content("success");
                    //注意：
                    //该种交易状态只在两种情况下出现
                    //1、开通了普通即时到账，买家付款成功后。
                    //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。

                    //1、开通了高级即时到账，买家付款成功后。
                }
            }
            return Content("");
        }

        [HttpPost]
        [Description("微信回调地址")]
        public async Task<ActionResult> WxNotifyUrl()
        {
            var loger = LogManager.GetLogger("Alipay");
            HttpContextBase context = HttpContext;
            WxPayData notifyData = new Notify(context).GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                loger.Error(GetType().ToString(), "The Pay result is error : " + res.ToXml());
                return Content(res.ToXml());
            }

            string transactionId = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryWxOrder(transactionId))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                loger.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                return Content(res.ToXml());
            }
            //查询订单成功
            else
            {
                string orderNo = notifyData.GetValue("out_trade_no").ToString();
                //await OrderContract.PayOrder(orderNo, PayType.微信);

                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                loger.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                return Content(res.ToXml());
            }
        }

        [Description("易宝支付页")]
        public ActionResult YeePay(string orderNo)
        {
            //var order = OrderContract.OrderInfos.Where(p => p.OrderNo == orderNo).SingleOrDefault();
            //if (order == null) return Content("订单不存在");

            string serverHost = ConfigurationManager.AppSettings["ServerHost"];

            //一键支付URL前缀
            string apiprefix = APIURLConfig.mobilePrefix;

            //网页支付地址
            string mobilepayURI = APIURLConfig.webpayURI;

            //商户账户编号
            string customernumber = YeePayConfig.merchantAccount;
            string hmacKey = YeePayConfig.merchantKey;
            string AesKey = YeePayConfig.AescKey;

            //日志字符串
            StringBuilder logsb = new StringBuilder();
            logsb.Append(DateTime.Now.ToString() + "\n");

            Random ra = new Random();
            string payproducttype = "ONEKEY"; // "支付方式";
            string amount = /*order.TotalPrice + */"";//支付金额为单位元       
            string requestid = orderNo;//订单号
            string productcat = "";//商品类别码，商户支持的商品类别码由易宝支付运营人员根据商务协议配置
            string productdesc = "订单商品";//商品描述
            string productname = "订单商品";//商品名称
            string assure = "0";//是否需要担保,1是，0否
            string divideinfo = "";//分账信息，格式”ledgerNo:分账比
            string bankid = "";//银行编码
            string period = "";//担保有效期，单位 ：天；当assure=1 时必填，最大值：30
            string memo = "";//商户备注
            string userno = /*order.UserInfo.Id +*/ "";//用户标识
            string ip = "";//IP
            string cardname = "";//持卡人姓名
            string idcard = "";//身份证
            string bankcardnum = "";//银行卡号

            //商户提供的商户后台系统异步支付回调地址
            string callbackurl = string.Format("{0}Admin/Pay/YeePayNotifyUrl", serverHost);
            //商户提供的商户前台系统异步支付回调地址
            string webcallbackurl = "";
            string hmac = "";


            hmac = Digest.GetHMAC(customernumber, requestid, amount, assure, productname, productcat, productdesc, divideinfo, callbackurl, webcallbackurl, bankid, period, memo, hmacKey);

            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("customernumber", customernumber);
            sd.Add("amount", amount);
            sd.Add("requestid", requestid);
            sd.Add("assure", assure);
            sd.Add("productname", productname);
            sd.Add("productcat", productcat);
            sd.Add("productdesc", productdesc);
            sd.Add("divideinfo", divideinfo);
            sd.Add("callbackurl", callbackurl);
            sd.Add("webcallbackurl", webcallbackurl);
            sd.Add("bankid", bankid);
            sd.Add("period", period);
            sd.Add("memo", memo);
            sd.Add("payproducttype", payproducttype);
            sd.Add("userno", userno);
            sd.Add("ip", ip);
            sd.Add("cardname", cardname);
            sd.Add("idcard", idcard);
            sd.Add("bankcardnum", bankcardnum);
            sd.Add("hmac", hmac);

            //将网页支付对象转换为json字符串
            string wpinfo_json = Newtonsoft.Json.JsonConvert.SerializeObject(sd);
            logsb.Append("手机支付明文数据json格式为：" + wpinfo_json + "\n");

            string datastring = AESUtil.Encrypt(wpinfo_json, AesKey);

            logsb.Append("手机支付业务数据经过AES加密后的值为：" + datastring + "\n");



            //打开浏览器访问一键支付网页支付链接地址，请求方式为get
            string postParams = "data=" + HttpUtility.UrlEncode(datastring) + "&customernumber=" + customernumber;
            string url = apiprefix + mobilepayURI + "?" + postParams;

            logsb.Append("手机支付链接地址为：" + url + "\n");

            string ybResult = YJPayUtil.payAPIRequest(apiprefix + mobilepayURI, datastring, false);

            logsb.Append("请求支付结果：" + ybResult + "\n");

            //将支付结果json字符串反序列化为对象
            RespondJson respJson = Newtonsoft.Json.JsonConvert.DeserializeObject<RespondJson>(ybResult);
            string yb_data = respJson.data;

            yb_data = AESUtil.Decrypt(yb_data, YeePayConfig.merchantKey);
            PayRequestJson result = Newtonsoft.Json.JsonConvert.DeserializeObject<PayRequestJson>(yb_data);
            if (result.code == 1)
            {
                bool r = Digest.PayRequestVerifyHMAC(result.customernumber, result.requestid, result.code, result.externalid, result.amount, result.payurl, hmacKey, result.hmac);
                if (r)
                {
                    //重定向跳转到易宝支付收银台
                    return Redirect(result.payurl);
                }
                else
                {
                    return Content("回调验签失败");
                }
            }
            else
            {
                return Content(result.msg);
            }
        }

        [Description("易宝回调地址")]
        public async Task<ActionResult> YeePayNotifyUrl()
        {
            try
            {
                if (Request["data"] == null)
                {
                    return Content("参数不正确");
                }
                //商户注意：接收到易宝的回调信息后一定要回写success用以保证握手成功！
                string data = Request["data"].ToString(); //回调中的参数data
                data = AESUtil.Decrypt(data, YeePayConfig.merchantKey);
                PayResultJson result = Newtonsoft.Json.JsonConvert.DeserializeObject<PayResultJson>(data);

                ///支付结果回调验签
                bool r = Digest.PayResultVerifyHMAC(result.customernumber, result.requestid, result.code, result.notifytype, result.externalid, result.amount, result.cardno, YeePayConfig.merchantKey, result.hmac);
                if (r && result.code == 1)
                {
                    //var opResult = await OrderContract.PayOrder(result.requestid, PayType.易宝);
                    //if (opResult.ResultType == OperationResultType.Success)
                    //{
                    //    return Content("SUCCESS");
                    //}
                }
                return Content("FAIL");
            }
            catch
            {
                return Content("支付失败！");
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 查询微信订单
        /// </summary>
        /// <param name="transactionId">微信支付订单号</param>
        /// <returns>订单是否存在</returns>
        private bool QueryWxOrder(string transactionId)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transactionId);
            WxPayData res = WxPayApi.OrderQuery(req);

            return res.GetValue("return_code").ToString() == "SUCCESS" &&
                   res.GetValue("result_code").ToString() == "SUCCESS";
        }

        #endregion
    }
}