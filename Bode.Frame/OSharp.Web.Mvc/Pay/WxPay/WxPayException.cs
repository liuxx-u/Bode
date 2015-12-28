using System;
using System.Collections.Generic;
using System.Web;

namespace OSharp.Web.Mvc.Pay.WxPay
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}