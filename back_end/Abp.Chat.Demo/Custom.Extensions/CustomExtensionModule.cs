using Custom.Extensions.AuthHelper;
using Custom.Extensions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System.Linq;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace Custom.Extensions
{
    /// <summary>
    /// 自定扩展模块
    /// </summary>
    public class CustomExtensionModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.Replace(
    ServiceDescriptor.Transient<IConventionalRouteBuilder, RouteBuilder>());
            services.Replace(
  ServiceDescriptor.Transient<IAbpServiceConvention, CustomApiConvert>());
            ConfigureRedisServices(services);
            services.AddTransient<TokenHelper>();
            //添加自定义用户凭证信息
            services.AddTransient<ICurrentUser, CustomUser>();
            services.AddTransient<ICustomUser, CustomUser>();
            //添加自定义异常
            Configure<MvcOptions>(options =>
            {
                var filterItem = options.Filters
                                        .Where(f => (f is ServiceFilterAttribute))
                                        .Cast<ServiceFilterAttribute>()
                                        .FirstOrDefault(f => f.ServiceType.Name.Equals("AbpExceptionFilter"));
                options.Filters.Remove(filterItem);
                //添加自己的异常过滤器
                options.Filters.AddService(typeof(GlobalExceptionFilter));
            });
        }
        private void ConfigureRedisServices(IServiceCollection services)
        {
            var section = services.GetConfiguration().GetSection("Redis:Default");
            var ip = section.GetSection("IP").Value;
            var port = section.GetSection("PORT").Value;
            var password = section.GetSection("Password").Value;
            var defaultDb = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
            var timeOut = int.Parse(section.GetSection("TimeOut").Value ?? "3000");

            var conf = new RedisConfiguration()
            {
                AbortOnConnectFail = true,
                Hosts = new RedisHost[]
                {
                    new RedisHost {Host = ip, Port = int.Parse(port ?? "6379")}
                 },
                Password = password,
                AllowAdmin = true,
                ConnectTimeout = timeOut,
                ServerEnumerationStrategy = new ServerEnumerationStrategy()
                {
                    Mode = ServerEnumerationStrategy.ModeOptions.All,
                    TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                    UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                }
            };

            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(conf);
        }

    }
}
