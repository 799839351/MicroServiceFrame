using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TXK.Framework.Core.Common.Log;

namespace TXK.API.Filter
{
    /// <summary>
    /// 获取Action执行时间
    /// </summary>
    public class TimingFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionTimer = GetTimer(context.HttpContext, "action");
            actionTimer.Stop();

            string controllerName = context.RouteData.Values["controller"].ToString() + "Controller";
            string actionName = context.RouteData.Values["action"].ToString();
            int duration = (int)actionTimer.ElapsedMilliseconds;
            LogHelper.Debug(this, string.Format("调用接口{0}结束，用时：{1}毫秒。", actionName, duration));
            //CommonLogInfo logInfo = new CommonLogInfo();
            //logInfo.LogType = LogMessageType.Debug;
            //logInfo.ClassName = filterContext.Controller.ToString();
            ////logInfo.ClassName = controllerName;
            //logInfo.MethodName = actionName;
            //logInfo.Message = string.Format("调用接口{0}结束，用时：{1}毫秒。", logInfo.MethodName, duration);
            //logInfo.Duration = duration;
            //logInfo.OperateSource = filterContext.HttpContext.Request.Headers["platform"];
            //logInfo.OperateID = filterContext.HttpContext.Request.Headers["clientVer"];
            ////logInfo.Type = filterContext.Controller.GetType();
            //logInfo.CallIP = GetHostAddress();
            //AppLog.Write(logInfo);



        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            GetTimer(context.HttpContext, "action").Start();

            var paras = context.ActionArguments;
            string strParas = string.Empty;
            foreach (var item in paras)
            {
                if (string.IsNullOrEmpty(strParas))
                {
                    strParas += item.Key + "=" + item.Value;
                }
                else
                {
                    strParas += "&" + item.Key + "=" + item.Value;
                }

            }
            if (string.IsNullOrEmpty(strParas))
            {
                strParas = "无";
            }
            string controllerName = context.RouteData.Values["controller"].ToString() + "Controller";
            string actionName = context.RouteData.Values["action"].ToString();
            LogHelper.Debug(this, string.Format("调用接口{0}开始。参数{1}", actionName, strParas));

            //CommonLogInfo logInfo = new CommonLogInfo();
            //logInfo.LogType = LogMessageType.Debug;
            //logInfo.ClassName = filterContext.Controller.ToString();
            //logInfo.MethodName = actionName;
            //logInfo.OperateSource = filterContext.HttpContext.Request.Headers["platform"];
            //logInfo.OperateID = filterContext.HttpContext.Request.Headers["clientVer"];
            ////logInfo.Type = filterContext.Controller.GetType();
            //logInfo.Message = string.Format("调用接口{0}开始。参数{1}。客户端：{2},版本号：{3}", logInfo.MethodName, strParas, filterContext.HttpContext.Request.Headers["platform"], filterContext.HttpContext.Request.Headers["clientVer"]);
            //logInfo.CallIP = GetHostAddress();
            //AppLog.Write(logInfo);
        }


        private Stopwatch GetTimer(HttpContext context, string name)
        {
            string key = "timer_" + name;
            if (context.Items.ContainsKey(key))
            {
                return (Stopwatch)context.Items[key];
            }

            var result = new Stopwatch();
            context.Items[key] = result;
            return result;
        }
    }
}
