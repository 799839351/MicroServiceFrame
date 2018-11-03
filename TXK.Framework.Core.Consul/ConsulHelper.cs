using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using TXK.Framework.Core.Common.Config;

namespace TXK.Framework.Core.Consul
{
    public static class ConsulHelper
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            ServiceDiscovery serviceDiscovery = ConfigHelper.GetSetting<ServiceDiscovery>("ServiceDiscovery");

            string _consulIP = serviceDiscovery.ConsulServer.Host;
            int _consulPort = serviceDiscovery.ConsulServer.Port;

            string _healthip = serviceDiscovery.HealthCheck.Host;
            int _healthport = serviceDiscovery.HealthCheck.Port;

            //string _ip = serviceDiscovery.ServiceInfo.Host;
            string _ip = serviceDiscovery.ServiceInfo.Host;
            int _port = serviceDiscovery.ServiceInfo.Port;

            try
            {
                if (!string.IsNullOrEmpty(_ip) && _port > 0)
                {
                    var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{_consulIP}:{_consulPort}"));//请求注册的 Consul 地址
                    var httpCheck = new AgentServiceCheck()
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(1),//服务启动多久后注册
                        Interval = TimeSpan.FromSeconds(serviceDiscovery.HealthCheck.Interval),//健康检查时间间隔，或者称为心跳间隔
                        HTTP = $"http://{_healthip}:{_healthport}" + serviceDiscovery.HealthCheck.API,//健康检查地址
                        Timeout = TimeSpan.FromSeconds(serviceDiscovery.HealthCheck.Timeout)
                    };
                    var registration = new AgentServiceRegistration()
                    {
                        Checks = new[] { httpCheck },
                        ID = serviceDiscovery.ServiceInfo.ServiceName + "_" + _ip + "_" + _port.ToString(),
                        //Name = env.ApplicationName,
                        Name = serviceDiscovery.ServiceInfo.ServiceName,
                        Address = _ip,
                        Port = _port,
                        Tags = new[] { $"urlprefix-/" + serviceDiscovery.ServiceInfo.ServiceName }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                        //Tags = new[] { $"urlprefix-/{env.ApplicationName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                    };
                    consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
                    lifetime.ApplicationStopping.Register(() =>
                    {
                        consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
                    });
                    return app;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"注册服务器{_consulIP}:{_consulPort}：", ex);
            }
            //}
            throw new Exception("服务（注册/发现）获取Host失败");
        }

    }
}
