// -----------------------------------------------------------------------
//  <copyright file="BaseController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-06-25 14:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.Web.Mvc;
using OSharp.Utility.Extensions;
using OSharp.Utility.Logging;
using OSharp.Web.Mvc.UI;
using OSharp.Web.Mvc.Logging;

namespace OSharp.Web.Mvc
{
    /// <summary>
    /// MVC控制器基类
    /// </summary>
    [OperateLogFilter]
    public abstract class BaseController : Controller
    {
        private readonly ILogger _logger;

        /// <summary>
        /// 基类构造函数
        /// </summary>
        protected BaseController()
        {
            _logger = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// 获取当前操作者Id
        /// </summary>
        protected int OperatorId
        {
            get
            {
                int operatorId = 0;
                int.TryParse(User.Identity.Name, out operatorId);
                return operatorId;
            }
        }

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
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            _logger.Error(exception.Message, exception);
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var message = "Ajax访问时引发异常：";
                if (exception is HttpAntiForgeryException)
                {
                    message += "安全性验证失败。<br>请刷新页面重试，详情请查看系统日志。";
                }
                else
                {
                    message += exception.Message;
                }
                filterContext.Result = Json(new AjaxResult(message, AjaxResultType.Error));
                filterContext.ExceptionHandled = true;
            }
        }
    }
}