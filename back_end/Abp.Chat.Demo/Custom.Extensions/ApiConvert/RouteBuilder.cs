using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Custom.Extensions
{
    public class RouteBuilder : ConventionalRouteBuilder
    {
        public RouteBuilder(IOptions<AbpConventionalControllerOptions> options) : base(options)
        {
        }
        public override string Build(string rootPath, string controllerName, ActionModel action, string httpMethod, ConventionalControllerSetting configuration)
        {

            var attr = GetActionCustomAttr(action);
            var path = GetActionCustomAttr(action)?.RelationPath;
            if (path == null)
            {
                path = action.ActionName;
            }
            return rootPath + path;
        }
        private static CustomApiAttribute GetActionCustomAttr(ActionModel action)
        {
            return action.ActionMethod.GetCustomAttributes(true).Where(a => typeof(CustomApiAttribute).IsAssignableFrom(a.GetType())).Cast<CustomApiAttribute>().FirstOrDefault();
        }
    }
}
