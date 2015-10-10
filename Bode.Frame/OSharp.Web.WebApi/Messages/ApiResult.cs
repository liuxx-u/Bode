using OSharp.Utility.Data;

namespace OSharp.Web.Http.Messages
{
    /// <summary>
    /// 表示WebApi操作结果 
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 初始化一个<see cref="ApiResult"/>类型的新实例
        /// </summary>
        public ApiResult(OperationResultType type = OperationResultType.Success, string content = "", object data = null)
            : this(content, data, type)
        { }

        /// <summary>
        /// 初始化一个<see cref="ApiResult"/>类型的新实例
        /// </summary>
        public ApiResult(string content, object data = null, OperationResultType type = OperationResultType.Success)
        {
            ReturnCode = type;
            ReturnMsg = content;
            ReturnData = data;
        }

        /// <summary>
        /// 获取 Api操作结果类型
        /// </summary>
        public OperationResultType ReturnCode { get; private set; }

        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string ReturnMsg { get; private set; }

        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public object ReturnData { get; private set; }
    }
}
