using OSharp.Utility.Data;
using OSharp.Utility.Extensions;

namespace OSharp.Web.Http.Messages
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ApiResultExtension
    {
        /// <summary>
        /// 操作结果转WebApi操作结果
        /// </summary>
        public static ApiResult ToApiResult(this OperationResult<object> result)
        {
            string content = result.Message ?? result.ResultType.ToDescription();
            return new ApiResult(result.ResultType, content, result.Data);
        }
    }
}
