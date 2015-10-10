using OSharp.Core.Context;
using OSharp.Utility.Extensions;
using OSharp.Web.Http.ModelBinders;
using System.Web.Http;


namespace OSharp.Web.Http
{
    /// <summary>
    /// WebAPI的控制器基类
    /// </summary>
    [MvcStyleBinding]
    //[CustomAuthorize]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// 序列化字符串
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="json">待序列化的字符串</param>
        /// <returns></returns>
        protected T JsonToEntity<T>(string json)
        {
            return json.FromJsonString<T>();
        }

        /// <summary>
        /// 获取当前操作者Id
        /// </summary>
        protected int OperatorId
        {
            get
            {
                int operatorId = 0;
                int.TryParse(OSharpContext.Current.Operator.UserId, out operatorId);
                return operatorId;
            }
        }

        /// <summary>
        /// 获取App每页数量
        /// </summary>
        protected int AppPageSize = 10;
    }
}
