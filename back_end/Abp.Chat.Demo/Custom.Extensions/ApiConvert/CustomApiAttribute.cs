using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomApiAttribute : ApiExplorerSettingsAttribute
    {
        public string Description { get; set; }
        public string RelationPath { get; set; }
        public string Method { get; set; } = "POST";

    }
}
