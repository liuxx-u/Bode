using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OSharp.Web.Mvc.Pay.WxPay
{
    /// <summary>
    /// App支付模式一回调处理类
    /// </summary>
    public class NativeNotify : Notify
    {
        public NativeNotify(HttpContextBase context)
            : base(context)
        {

        }

        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                httpContext.Response.Write(res.ToXml());
                httpContext.Response.End();
            }

            string transactionId = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transactionId))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                httpContext.Response.Write(res.ToXml());
                httpContext.Response.End();
            }
            //查询订单成功
            else
            {



                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                httpContext.Response.Write(res.ToXml());
                httpContext.Response.End();
            }
        }

        //查询订单
        private bool QueryOrder(string transactionId)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transactionId);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}