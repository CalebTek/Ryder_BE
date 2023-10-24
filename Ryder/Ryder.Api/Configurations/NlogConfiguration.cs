using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Ryder.Api.Configurations
{
    public static class NLogConfiguration
    {
        private static NLogLoggingConfiguration _logConfig;

        public static void AddNLogging(HostBuilderContext hostBuilderContext, ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            if (_logConfig == null)
            {
                _logConfig = new NLogLoggingConfiguration(hostBuilderContext.Configuration.GetSection("NLog"));
                LogManager.Configuration = _logConfig;
            }

            loggingBuilder.AddNLog(_logConfig);
        }
    }
}