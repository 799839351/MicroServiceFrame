using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace TXK.Framework.Core.Common.Net
{
    public class EnvironmentHelper
    {
        //private static string machineName = string.Empty;

        ///// <summary>
        ///// 获取此本地计算机的 NetBIOS 名称。 
        ///// </summary>
        //public static string MachineName
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(machineName))
        //        {
        //            string machineName = string.Empty;
        //            try
        //            {
        //                machineName = Environment.MachineName;
        //            }
        //            catch (Exception e)
        //            {
        //                //machineName = String.Format(SR.Culture, SR.IntrinsicPropertyError, e.Message);
        //            }
        //        }
        //        return machineName;
        //    }
        //}

        private static string machineFullName = string.Empty;
        /// <summary>
        /// 获取此本地计算机的 NetBIOS 全名称。 
        /// </summary>
        public static string MachineFullName
        {
            get
            {
                if (string.IsNullOrEmpty(machineFullName))
                {
                    machineFullName = Dns.GetHostName();
                }
                return machineFullName;
            }
        }

        private static string machineIPAddress = string.Empty;

        public static string MachineIPAddress
        {
            get
            {
                if (string.IsNullOrEmpty(machineIPAddress))
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        foreach (NetworkInterface netInt in NetworkInterface.GetAllNetworkInterfaces())
                        {
                            if (netInt.Name == "eth0")
                            {
                                IPInterfaceProperties property = netInt.GetIPProperties();
                                foreach (UnicastIPAddressInformation ip in property.UnicastAddresses)
                                {
                                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                    {
                                        machineIPAddress = ip.Address.MapToIPv4().ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        IPAddress[] address = Dns.GetHostEntry(MachineFullName).AddressList;
                        if (address != null)
                        {
                            foreach (IPAddress addr in address)
                            {
                                //过滤IPv6的地址信息
                                if (addr.ToString().Length <= 16 && addr.ToString().Length > 5)
                                {
                                    machineIPAddress = addr.ToString();
                                    break;
                                }
                            }
                        }
                    }


                }
                return machineIPAddress;
            }
        }

        ///// <summary>
        ///// 获取些本地计算机所在域的名称。
        ///// </summary>
        //public static string DomainName
        //{
        //    get
        //    {
        //        string domainName = string.Empty;
        //        string hostName = MachineFullName;
        //        if (hostName.StartsWith(MachineName, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            domainName = hostName.Substring(MachineName.Length + 1);
        //        }

        //        return domainName;
        //    }
        //}

        ///// <summary>
        ///// 获取当前计算机上的处理器数。 
        ///// </summary>
        //public static int ProcessorCount
        //{
        //    get { return Environment.ProcessorCount; }
        //}

        ///// <summary>
        ///// 应用程序域名。
        ///// </summary>
        //public static string AppDomainName
        //{
        //    get
        //    {
        //        string appDomainName = string.Empty;
        //        try
        //        {
        //            appDomainName = AppDomain.CurrentDomain.FriendlyName;
        //        }
        //        catch (Exception e)
        //        {
        //            //appDomainName = String.Format(SR.Culture, SR.IntrinsicPropertyError, e.Message);
        //        }

        //        return appDomainName;
        //    }
        //}

        ///// <summary>
        ///// 获取启动当前线程的人的域用户名。 
        ///// </summary>
        //public static string DomainUserName
        //{
        //    get
        //    {
        //        string domainUserName = string.Empty;
        //        try
        //        {
        //            if (string.IsNullOrEmpty(domainUserName))
        //            {
        //                domainUserName = string.Format("{0}/{1}", Environment.UserDomainName, Environment.UserName);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //domainUserName = string.Format(SR.Culture, SR.IntrinsicPropertyError, ex.Message);
        //        }

        //        return domainUserName;
        //    }
        //}

        #region [ Process Info ]

        ///// <summary>
        ///// 进程代号。
        ///// </summary>
        //public static string ProcessId
        //{
        //	get
        //	{
        //		string processId = string.Empty;
        //		try
        //		{
        //			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
        //			using (Process process = Process.GetCurrentProcess())
        //			{
        //				processId = process.Id.ToString();
        //			}
        //		}
        //		catch (Exception ex)
        //		{
        //			processId = String.Format(SR.Culture, SR.IntrinsicPropertyError, ex.Message);
        //		}

        //		return processId;
        //	}
        //}

        ///// <summary>
        ///// 进程名称。
        ///// </summary>
        //public static string ProcessName
        //{
        //	get
        //	{
        //		string processName = string.Empty;
        //		try
        //		{
        //			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
        //			using (Process process = Process.GetCurrentProcess())
        //			{
        //				processName = process.ProcessName;
        //			}
        //		}
        //		catch (Exception ex)
        //		{
        //			processName = String.Format(SR.Culture, SR.IntrinsicPropertyError, ex.Message);
        //		}

        //		return processName;
        //	}
        //}

        #endregion

        #region [ Thread Info ]

        ///// <summary>
        ///// 线程代码。
        ///// </summary>
        //public static int ThreadId
        //{
        //    get { return Thread.CurrentThread.ManagedThreadId; }
        //}

        ///// <summary>
        ///// 线程名称。
        ///// </summary>
        //public static string ThreadName
        //{
        //    get { return Thread.CurrentThread.Name; }
        //}

        #endregion



    }
}
