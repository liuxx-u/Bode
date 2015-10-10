using Bode.Adapter.Push.Jpush.common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bode.Adapter.Push.Jpush.device
{
   public class TagListResult:BaseResult
    {
        public List<String> tags ;
        public TagListResult()
        {
            tags = null;
        }
        public override bool isResultOK()
        {
            if (Equals(ResponseResult.responseCode, HttpStatusCode.OK))
            {
                return true;
            }
            return false;
        }
        public static TagListResult fromResponse(ResponseWrapper responseWrapper)
        {
            TagListResult tagListResult = new TagListResult();
            if (responseWrapper.isServerResponse())
            {
                tagListResult = JsonConvert.DeserializeObject<TagListResult>(responseWrapper.responseContent);
            }
            tagListResult.ResponseResult = responseWrapper;
            return tagListResult;
        }
    }
}
