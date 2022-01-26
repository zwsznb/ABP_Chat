using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Logging;

namespace Custom.Extensions
{
    public class AbpApiException : Exception,
        IHasErrorCode,
        IHasErrorDetails,
        IHasLogLevel
    {
        public AbpApiException(string details, string code = "500")
        {
            this.Details = details;
            this.Code = code;
        }
        public string Code { get; set; }

        public string Details { get; set; }

        public LogLevel LogLevel { get; set; } = LogLevel.Error;
    }
}
