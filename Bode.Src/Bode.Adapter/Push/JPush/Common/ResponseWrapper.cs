using System;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Bode.Adapter.Push.Jpush.push;
using OSharp.Utility.Extensions;

namespace Bode.Adapter.Push.Jpush.common
{
    public class ResponseWrapper
    {
        private const int RESPONSE_CODE_NONE = -1;
    
        //private static Gson _gson = new Gson();
        public JpushError jpushError;

        public HttpStatusCode responseCode = HttpStatusCode.BadRequest;
        private String _responseContent;
        public String responseContent
        {
            get
            {
                return _responseContent;
            }
            set
            {
                _responseContent = value;
            }
        }
        public void setErrorObject()
        {
            if(!string.IsNullOrEmpty(_responseContent))
            {
                 jpushError = JsonConvert.DeserializeObject<JpushError>(_responseContent);
            }
        }

        public int rateLimitQuota;
        public int rateLimitRemaining;
        public int rateLimitReset;

        public bool isServerResponse()
        {
            return responseCode == HttpStatusCode.OK;
        }
        public String exceptionString;

	    public ResponseWrapper() {
	    }
        public void setRateLimit(String quota, String remaining, String reset) {
            if (null == quota) return;
            try
            {
                if (quota != "" &&  quota.IsNumeric())
                {
                    rateLimitQuota = int.Parse(quota);
                }
                if (remaining != "" && quota.IsNumeric())
                {
                    rateLimitRemaining = int.Parse(remaining);
                }
                if (reset != "" && quota.IsNumeric())
                {
                    rateLimitReset = int.Parse(reset);
                }
                Console.WriteLine(string.Format("JPush API Rate Limiting params - quota:{0}, remaining:{1}, reset:{2} ", quota, remaining, reset) +" "+ DateTime.Now);
            }
            catch(Exception e)
            {
                Debug.Print(e.Message);
            }
        }

    }
}
