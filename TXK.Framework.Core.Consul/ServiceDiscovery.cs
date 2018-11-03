using System;
using System.Collections.Generic;
using System.Text;

namespace TXK.Framework.Core.Consul
{
    public class ServiceDiscovery
    {
        public ServiceDiscovery()
        {
            ConsulServer = new ConsulServer();
            HealthCheck = new HealthCheck();
            ServiceInfo = new ServiceInfo();
        }

        public ConsulServer ConsulServer { get; set; }
        public HealthCheck HealthCheck { get; set; }
        public ServiceInfo ServiceInfo { get; set; }
    }

    public class ConsulServer
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class HealthCheck
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string API { get; set; }
        public int Interval { get; set; }
        public int Timeout { get; set; }
    }

    public class ServiceInfo
    {
        public string ServiceName { get; set; }
        public string ServiceID { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

    }
}
