using Serilog;

namespace DB
{
    public class LoggerManager
    {
        private static LoggerManager _instance;

        public ILogger Logger { get; }

        private LoggerManager()
        {
            var logFileName = "log.txt";
            
            Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(logFileName, rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }

        public static LoggerManager Instance => _instance ??= new LoggerManager();
    }

}
