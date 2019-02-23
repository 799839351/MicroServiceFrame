using log4net;
using log4net.Config;
using log4net.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TXK.Framework.Core.Common.Net;

namespace TXK.Framework.Core.Common.Log
{
    public enum LogLevel
    {
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Fatal = 5
    }

    public class LogEntity
    {
        public string LogID { get; set; }
        public string LogTime { get; set; }
        public LogLevel LogLevel { get; set; }
        public string LogServerIP { get; set; }
        public object Content { get; set; }
        public Exception Exception { get; set; }
        public string Source { get; set; }

        public Int32 Port { get; set; }
        
    }

    public static class LogHelper
    {
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, ILog> Loggers = new System.Collections.Concurrent.ConcurrentDictionary<Type, ILog>();

        static LogHelper()
        {
            //ILoggerRepository repository = log4net.LogManager.CreateRepository("NETCoreRepository");
            ILoggerRepository repository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// 获取记录器
        /// </summary>
        /// <param name="source">soruce</param>
        /// <returns></returns>
        private static ILog GetLogger(Type source)
        {
            if (Loggers.ContainsKey(source))
            {
                return Loggers[source];
            }
            else
            {
                ILog logger = LogManager.GetLogger(source);
                //ILog logger = LogManager.GetLogger(Startup.repository.Name,source);
                Loggers.TryAdd(source, logger);
                return logger;
            }
        }

        /* Log a message object */

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Debug(object source, string message)
        {
            Debug(source.GetType(), message);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="ps">ps</param>
        public static void Debug(object source, string message, params object[] ps)
        {
            Debug(source.GetType(), string.Format(message, ps));
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Debug(Type source, string message)
        {
            WriteLog(source, LogLevel.Debug, message, null);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Info(object source, object message)
        {
            Info(source.GetType(), message);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Info(Type source, object message)
        {
            WriteLog(source, LogLevel.Info, message, null);

        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Warn(Type source, object message)
        {
            WriteLog(source, LogLevel.Warn, message, null);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Error(object source, object message)
        {
            Error(source.GetType(), message);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Error(Type source, object message)
        {
            WriteLog(source, LogLevel.Error, message, null);

        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Fatal(object source, object message)
        {
            Fatal(source.GetType(), message);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Fatal(Type source, object message)
        {
            WriteLog(source, LogLevel.Fatal, message, null);
        }

        /* Log a message object and exception */

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Debug(object source, object message, Exception exception)
        {
            Debug(source.GetType(), message, exception);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Debug(Type source, object message, Exception exception)
        {
            WriteLog(source, LogLevel.Debug, message, exception);
            //GetLogger(source).Debug(message, exception);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Info(object source, object message, Exception exception)
        {
            Info(source.GetType(), message, exception);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Info(Type source, object message, Exception exception)
        {
            WriteLog(source, LogLevel.Info, message, exception);
            //GetLogger(source).Info(message, exception);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Warn(object source, object message, Exception exception)
        {
            Warn(source.GetType(), message, exception);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Warn(Type source, object message, Exception exception)
        {
            WriteLog(source, LogLevel.Warn, message, exception);
            //GetLogger(source).Warn(message, exception);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Error(object source, object message, Exception exception)
        {
            Error(source.GetType(), message, exception);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Error(Type source, object message, Exception exception)
        {
            WriteLog(source, LogLevel.Error, message, exception);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Fatal(object source, object message, Exception exception)
        {
            Fatal(source.GetType(), message, exception);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Fatal(Type source, object message, Exception exception)
        {
            WriteLog(source, LogLevel.Fatal, message, exception);
        }

        public static void WriteLog(Type source, LogLevel logLevel, object message, Exception exception)
        {
            ILog logger = GetLogger(source);
            LogEntity logEntity = new LogEntity();
            logEntity.LogID = Guid.NewGuid().ToString();
            logEntity.Content = message;
            logEntity.LogTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            logEntity.Exception = exception;
            logEntity.Source = source.ToString();
            logEntity.LogLevel = logLevel;
            logEntity.LogServerIP = EnvironmentHelper.MachineIPAddress;
            
            string content = JsonConvert.SerializeObject(logEntity);

            switch (logLevel)
            {
                case LogLevel.Error:
                    if (logger.IsErrorEnabled)
                    {
                        logger.Error(content);
                    }
                    break;
                case LogLevel.Info:
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info(content);
                    }
                    break;
                case LogLevel.Debug:
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug(content);
                    }
                    break;
                case LogLevel.Warn:
                    if (logger.IsWarnEnabled)
                    {
                        logger.Warn(content);
                    }
                    break;
                case LogLevel.Fatal:
                    if (logger.IsFatalEnabled)
                    {
                        logger.Fatal(content);
                    }
                    break;
                default:
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug(content);
                    }
                    break;
            }

        }
    }
}

