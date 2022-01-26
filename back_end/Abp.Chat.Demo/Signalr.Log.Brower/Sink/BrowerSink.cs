using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;
using Signalr.Log.Brower.Services;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Signalr.Log.Brower.Sink
{
    public class BrowerSink : ILogEventSink, ISingletonDependency
    {
        private readonly List<string> levelUpString = new() { "VER", "DEG", "INF", "WRN", "ERR", "FAT" };
        private readonly IBrowerLogServices browerLogServices;
        public BrowerSink(IBrowerLogServices _browerLogServices)
        {
            browerLogServices = _browerLogServices;
        }
        public void Emit(LogEvent logEvent)
        {
            var str = FormatLog(logEvent);
            browerLogServices.Enqueue(str);
        }
        public string FormatLog(LogEvent log)
        {
            var MsgTemplate = log.MessageTemplate.Text;
            var parser = new MessageTemplateParser();
            var template = parser.Parse(MsgTemplate);
            var format = new StringBuilder();
            var time = $"[{log.Timestamp:HH:mm:ss} {GetLogLevelToUp(log.Level)}] ";
            format.Append(time);
            var index = 0;
            foreach (var tok in template.Tokens)
            {
                if (tok is TextToken)
                    format.Append(tok);
                else
                    format.Append("{" + index++ + "}");
            }
            var netStyle = format.ToString();
            return netStyle;
        }
        public string GetLogLevelToUp(LogEventLevel level)
        {
            return levelUpString[(int)level];
        }
    }
}
