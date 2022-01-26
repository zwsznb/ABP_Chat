using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Signalr.Log.Brower.Services;
using Signalr.Log.Brower.Sink;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace Signalr.Log.Brower
{
    /// <summary>
    /// 浏览器日志模块，通过signalr向外通信
    /// </summary>
    [DependsOn(typeof(AbpAspNetCoreSignalRModule))]
    public class BrowerLogModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<ILogEventSink, BrowerSink>();
            context.Services.AddSingleton<IBrowerLogServices, BrowerLogServices>();
        }
    }
}
