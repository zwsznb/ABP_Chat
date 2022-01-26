using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Custom.Extensions
{
    public class CustomApiConvert : AbpServiceConvention
    {
        public CustomApiConvert(IOptions<AbpAspNetCoreMvcOptions> options, IConventionalRouteBuilder conventionalRouteBuilder) : base(options, conventionalRouteBuilder)
        {
        }
        protected override void ConfigureApiExplorer(ControllerModel controller)
        {
            var attr = controller.ControllerType.GetSingleAttributeOrNull<ApiExplorerSettingsAttribute>();
            if (attr == null)
            {
                controller.ApiExplorer.IsVisible = false;
                return;
            }
            controller.ApiExplorer.GroupName = attr.GroupName;
            if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            foreach (var action in controller.Actions)
            {
                ConfigureApiExplorer(action);
            }
        }
        protected override void AddAbpServiceSelector(string rootPath, string controllerName, ActionModel action, ConventionalControllerSetting configuration)
        {
            //只有带有custom特性的方法才被添加为api
            //根据action名称判断请求方式
            //获取自定义属性
            CustomApiAttribute attr = GetActionCustomAttr(action);
            if (attr == null)
            {
                return;
            }
            var abpServiceSelectorModel = new SelectorModel
            {
                AttributeRouteModel = CreateAbpServiceAttributeRouteModel(rootPath, controllerName, action, attr.Method, configuration),
                ActionConstraints = { new HttpMethodActionConstraint(new[] { attr.Method }) }
            };

            action.Selectors.Add(abpServiceSelectorModel);
        }

        private static CustomApiAttribute GetActionCustomAttr(ActionModel action)
        {
            return action.ActionMethod.GetCustomAttributes(true).Where(a => typeof(CustomApiAttribute).IsAssignableFrom(a.GetType())).Cast<CustomApiAttribute>().FirstOrDefault();
        }

        protected override void NormalizeSelectorRoutes(string rootPath, string controllerName, ActionModel action, ConventionalControllerSetting configuration)
        {
            foreach (var selector in action.Selectors)
            {
                var attr = GetActionCustomAttr(action);
                if (attr == null)
                {
                    return;
                }

                if (selector.AttributeRouteModel == null)
                {
                    selector.AttributeRouteModel = CreateAbpServiceAttributeRouteModel(rootPath, controllerName, action, attr.Method, configuration);
                }

                if (!selector.ActionConstraints.OfType<HttpMethodActionConstraint>().Any())
                {
                    selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { attr.Method }));
                }
            }
        }
    }
}
