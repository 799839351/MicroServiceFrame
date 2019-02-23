using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using TXK.API.MiddleWare;
using TXK.Framework.Core.Consul;

namespace TXK.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            ////第一种方式加载配置
            //services.AddSingleton(Configuration);

            ////第二种方式加载配置
            ////加载配置文件
            //services.AddOptions();
            //services.Configure<List<DatabaseItem>>(Configuration.GetSection("DatabaseOptions"));

            #region Swagger UI相关配置
            services.AddSwaggerGen(c =>
            {
                //配置第一个Doc
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "MyService",
                        Version = "v1",
                        Description = "地址接口服务",
                        Contact = new Contact { Email = "799839351@qq.com", Name = "谭小康", Url = "https://www.baidu.com" },
                    });
                ////配置第二个Doc
                //c.SwaggerDoc("v2", new Info { Title = "My API_2", Version = "v2" });
                //Determine base path for the application.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //Set the comments path for the swagger json and ui.
                var xmlPath = Path.Combine(basePath, "TXK.API.xml");
                c.IncludeXmlComments(xmlPath);
            });
            #endregion
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger UI相关
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyService");
                c.RoutePrefix = "swagger";

            });
            app.UseSwagger();
            #endregion

            #region 启动时，默认注册Consul服务器
            //  app.RegisterConsul(lifetime);
            #endregion


            //异常处理中间件
            app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));


            app.UseMvc();
        }
    }
}
