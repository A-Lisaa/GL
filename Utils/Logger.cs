using Serilog;

namespace Utils {
    public static class Logger {
        public static Serilog.Core.Logger GetLogger(bool debugEnabled = false, bool consoleLog = false) {
            var loggerConfiguration = new LoggerConfiguration();

            if (consoleLog) {
                loggerConfiguration.WriteTo.Console();
            }
            else {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                // I suspect that retainedFileCountLimit doesn't work, and we don't want like 1488 log files
                loggerConfiguration.WriteTo.File($"logs/{timestamp}.log", retainedFileCountLimit: 5);
            }

#if DEBUG
            loggerConfiguration.MinimumLevel.Debug();
#endif
            if (debugEnabled) {
                loggerConfiguration.MinimumLevel.Debug();
            }

            return loggerConfiguration.CreateLogger();
        }
    }
}
