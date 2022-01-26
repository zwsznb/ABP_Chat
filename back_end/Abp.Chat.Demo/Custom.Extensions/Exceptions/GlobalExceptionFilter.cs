using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Validation;

namespace Custom.Extensions
{
    //可能后面做自定义异常处理用的到
    public class GlobalExceptionFilter : AbpExceptionFilter
    {
        //这里只是把源码简单的复制过来，可以自定义
        protected override async Task HandleAndWrapException(ExceptionContext context)
        {
            var exceptionHandlingOptions = context.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;
            var exceptionToErrorInfoConverter = context.GetRequiredService<IExceptionToErrorInfoConverter>();
            var remoteServiceErrorInfo = exceptionToErrorInfoConverter.Convert(context.Exception, exceptionHandlingOptions.SendExceptionsDetailsToClients);

            var logLevel = context.Exception.GetLogLevel();

            var remoteServiceErrorInfoBuilder = new StringBuilder();
            remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
            remoteServiceErrorInfoBuilder.AppendLine(context.GetRequiredService<IJsonSerializer>().Serialize(remoteServiceErrorInfo, indented: true));

            var logger = context.GetService<ILogger<AbpExceptionFilter>>(NullLogger<AbpExceptionFilter>.Instance);

            logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());

            logger.LogException(context.Exception, logLevel);

            await context.GetRequiredService<IExceptionNotifier>().NotifyAsync(new ExceptionNotificationContext(context.Exception));
            //格式化请求头
            context.HttpContext.Response.Headers.Add(AbpHttpConsts.AbpErrorFormat, "true");
            //响应码
            //自定义全局异常
            if (context.Exception is AbpApiException)
            {
                var exception = context.Exception as AbpApiException;
                context.HttpContext.Response.StatusCode = Convert.ToInt16(exception.Code);
                context.Result = new ObjectResult(ApiResult<object>.GetResult(new { error = exception.Details }, 10002));
            }
            //模型认证异常
            else if (context.Exception is AbpValidationException)
            {
                var exception = context.Exception as AbpValidationException;
                var code = (int)context
                    .GetRequiredService<IHttpExceptionStatusCodeFinder>()
                    .GetStatusCode(context.HttpContext, context.Exception);
                context.HttpContext.Response.StatusCode = code;
                //对验证信息进行整合处理
                context.Result = new ObjectResult(ApiResult<object>.GetResult(new { error = exception.ValidationErrors }, 10001));
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)context
                    .GetRequiredService<IHttpExceptionStatusCodeFinder>()
                    .GetStatusCode(context.HttpContext, context.Exception);
                //响应信息
                context.Result = new ObjectResult(new RemoteServiceErrorResponse(remoteServiceErrorInfo));

            }

            context.Exception = null; //Handled!
        }
    }
}


