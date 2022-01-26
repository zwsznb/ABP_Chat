using Abp.Chat.Demo.Application;
using Abp.Chat.Demo.Application.ChatManager;
using Abp.Chat.Demo.Application.FriendManager;
using Abp.Chat.Demo.Application.Test;
using Abp.Chat.Demo.Application.UserManager;
using Abp.Chat.Demo.EntityFrameworkCore.Migrations;
using Custom.Extensions;
using Custom.Extensions.AuthHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Signalr.Log.Brower;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Abp.Chat.Demo
{
    [DependsOn(typeof(AbpAspNetCoreModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(AbpAspNetCoreAuthenticationJwtBearerModule))]
    [DependsOn(typeof(AbpAuthorizationModule))]
    [DependsOn(typeof(CustomExtensionModule))]
    [DependsOn(typeof(AbpChatDemoEntityFrameworkCoreMigrationModule))]
    [DependsOn(typeof(AbpChatDemoApplicationModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpAuditingModule))]
    [DependsOn(typeof(BrowerLogModule))]
    public class AppModule : AbpModule
    {
        private readonly string CorsString = "Any";
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddControllers();
            ConfigurationSwagger(context);
            ConfigAuthentication(context.Services);
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options
                    .ConventionalControllers
                    .Create(typeof(AbpChatDemoApplicationModule).Assembly, opts =>
                    {
                        //去除自动添加api
                        opts.TypePredicate = type => { return false; };
                        //手动添加api
                        opts.ControllerTypes.Add(typeof(TestService));
                        opts.ControllerTypes.Add(typeof(AccountService));
                        opts.ControllerTypes.Add(typeof(FriendInvitationService));
                        opts.ControllerTypes.Add(typeof(FriendManagerService));
                        opts.ControllerTypes.Add(typeof(ChatManagerService));
                        opts.RootPath = "AbpChat";
                    });
            });
            Configure<AbpAuditingOptions>(options =>
            {
                options.Contributors.Add(new AspNetCoreAuditLogContributor());
                options.IsEnabledForGetRequests = true;
            });
            ConfigurationCors(context.Services);
        }
        private void ConfigurationCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsString, builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
        }

        private static void ConfigurationSwagger(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(
                              options =>
                              {
                                  options.SwaggerDoc("v1", new OpenApiInfo { Title = "Abp.Chat.Demo", Version = "v1" });
                                  options.DocInclusionPredicate((docName, description) => true);
                                  options.CustomSchemaIds(type => type.FullName);
                                  options.OperationFilter<SwaggerApiFilterDisc>();
                                  //Bearer 的scheme定义
                                  var securityScheme = new OpenApiSecurityScheme()
                                  {
                                      Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                                      Name = "Authorization",
                                      //参数添加在头部
                                      In = ParameterLocation.Header,
                                      //使用Authorize头部
                                      Type = SecuritySchemeType.Http,
                                      //内容为以 bearer开头
                                      Scheme = "bearer",
                                      BearerFormat = "JWT"
                                  };
                                  //把所有方法配置为增加bearer头部信息
                                  var securityRequirement = new OpenApiSecurityRequirement()
                                  {
                                      {
                                          new OpenApiSecurityScheme
                                          {
                                              Reference = new OpenApiReference
                                              {
                                                  Type = ReferenceType.SecurityScheme,
                                                  Id = "bearerAuth"
                                              }
                                          },
                                          new string[] { }
                                      }
                                  };
                                  //注册到swagger中
                                  options.AddSecurityDefinition("bearerAuth", securityScheme);
                                  options.AddSecurityRequirement(securityRequirement);
                              }
                          );
        }
        //配置jwt认证
        private static void ConfigAuthentication(IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            services.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
            var tokenOptions = new TokenOptions();
            configuration.Bind("TokenOptions", tokenOptions);
            //添加jwt认证,首先创建默认的scheme
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //如果自己不设置这个名称就会用默认的jwt名称,然后可以进行配置JwtBearerOptions
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.Secret))
                };
                //如果不自定义会执行默认的事件
                options.Events = new JwtBearerEvents();
                //这个处理事件会在获取身份凭证失败的时候执行，或者说token无法被处理，token格式不对等，内部异常已经被捕获了
                options.Events.OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("无法处理token");
                    return Task.CompletedTask;
                };
                options.Events.OnMessageReceived = async context =>
                 {
                     var accessToken = context.Request.Query["access_token"];
                     // If the request is for our hub...
                     var path = context.HttpContext.Request.Path;
                     if (!string.IsNullOrEmpty(accessToken) &&
                         (path.StartsWithSegments("/abpChatHub")))
                     {
                         // Read the token out of the query string
                         context.Token = accessToken;
                     }
                     await Task.CompletedTask;
                 };
                //未登录处理事件，在授权中间件执行时才有会触发
                options.Events.OnChallenge = async context =>
                {
                    //终止响应
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(ApiResult<object>.GetResult(new { data = "认证失败" }, 10011)));
                };
                //没有权限处理，跟未登录事件处理差不多的
                options.Events.OnForbidden = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(ApiResult<object>.GetResult(new { data = "没有权限或者失效" }, 10012)));
                };
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var env = context.GetEnvironment();
            var app = context.GetApplicationBuilder();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abp.Chat.Demo v1"));
            }

            app.UseRouting();
            app.UseCors(CorsString);
            app.UseJwtTokenMiddleware();
            app.UseAuthorization();
            app.UseAuditing();

            app.UseConfiguredEndpoints();
        }
    }
    /// <summary>
    /// api注释显示
    /// </summary>
    public class SwaggerApiFilterDisc : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            MethodInfo method = null;
            var attr = context.ApiDescription.CustomAttributes().Where(a => typeof(CustomApiAttribute).IsAssignableFrom(a.GetType())).Cast<CustomApiAttribute>().FirstOrDefault();
            operation.Description = attr.Description;
            operation.Summary = attr.Description;
            context.ApiDescription.TryGetMethodInfo(out method);
            var groupName = method.DeclaringType.GetSingleAttributeOrNull<ApiExplorerSettingsAttribute>().GroupName;
            operation.Tags = new List<OpenApiTag>() { new OpenApiTag { Name = groupName } };
        }
    }
}
