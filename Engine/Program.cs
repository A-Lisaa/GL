using Serilog;

namespace Engine {
    internal static class Program {
        public static void Main() {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
