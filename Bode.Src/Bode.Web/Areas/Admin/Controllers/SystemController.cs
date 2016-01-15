using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Bode.Services.Core.Contracts;
using OSharp.Core.Logging;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Web.Mvc.Security;
using OSharp.Web.Mvc.UI;
using OSharp.Web.Mvc;
using OSharp.Utility.Windows;

namespace Bode.Web.Areas.Admin.Controllers
{
    //[Authorize]
    [Description("系统管理")]
    public class SystemController : AdminBaseController
    {
        /// <summary>
        /// 获取或设置 日志业务对象
        /// </summary>
        public ILoggingContract LoggingContract { get; set; }

        #region Ajax功能

        #region 操作日志

        [AjaxOnly]
        [Description("获取操作日志数据")]
        public ActionResult GetOperateLogData()
        {
            int total;
            GridRequest request = new GridRequest(Request);
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("CreatedTime", ListSortDirection.Descending) };
            }
            var datas = GetQueryData<OperateLog, int>(LoggingContract.OperateLogs.Include(p => p.DataLogs), out total, request).Select(m => new
            {
                m.Id,
                m.FunctionName,
                m.Operator.UserId,
                m.Operator.UserName,
                m.Operator.Ip,
                m.CreatedTime
            }).ToList();
            return Json(new GridData<object>(datas, total), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [Description("删除操作日志数据")]
        public ActionResult DeleteOperateLogs(int[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = LoggingContract.DeleteOperateLogs(ids);
            return Json(result.ToAjaxResult());
        }

        [AjaxOnly]
        [Description("获取数据日志数据")]
        public ActionResult GetDataLogData(int operateLogId)
        {
            var data = LoggingContract.DataLogs.Where(p => p.OperateLog != null && p.OperateLog.Id == operateLogId).Select(m => new
            {
                m.Id,
                m.Name,
                m.EntityName,
                m.OperateType,
                LogItems = m.LogItems.Select(n => new
                {
                    n.Field,
                    n.FieldName,
                    n.OriginalValue,
                    n.NewValue,
                    n.DataType
                })
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 视图页面

        [Description("操作日志列表")]
        public ActionResult OperateLogList()
        {
            ViewBag.OperatingTypes = typeof(OperatingType).ToDictionary().Select(p => new
            {
                val = p.Key,
                text = p.Value
            }).ToList();
            return View();
        }

        [Description("服务器信息")]
        public ActionResult ServerInfo()
        {
            ViewBag.SystemInfo = SystemInfoHandler.GetSystemInfo();
            return View();
        }

        #endregion

    }
}