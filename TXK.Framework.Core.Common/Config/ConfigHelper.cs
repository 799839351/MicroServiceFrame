using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

namespace TXK.Framework.Core.Common.Config
{

    public class ConfigHelper
    {
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, IConfiguration> Configs = new System.Collections.Concurrent.ConcurrentDictionary<string, IConfiguration>();

        private static IConfiguration GetConfig(string fileName)
        {
            if (Configs.ContainsKey(fileName))
            {
                return Configs[fileName];
            }
            else
            {
                IConfiguration config = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .Add(new JsonConfigurationSource { Path = fileName, Optional = false, ReloadOnChange = true })
                               .Build();
                Configs.TryAdd(fileName, config);
                return config;
            }
        }


        /// <summary>
        /// 获取配置节点对象
        /// </summary>   
        public static T GetSetting<T>(string key, string fileName = "appsettings.json") where T : class, new()
        {
            IConfiguration config = GetConfig(fileName);
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }


        /// <summary>
        /// 获取配置节点对象
        /// </summary>   
        public static string GetStringValue(string key, string fileName = "appsettings.json")
        {
            IConfiguration config = GetConfig(fileName);
            return config[key];
        }

    }
}
