namespace MVP.Server
{
    public static class Logger
    {
        private static readonly string LogPath = Path.Combine(AppContext.BaseDirectory, "logs", "server.log");
        private static readonly object _lock = new object();

        static Logger()
        {
            var dir = Path.GetDirectoryName(LogPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public static void Log(string level, string message)
        {
            var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            Console.WriteLine(line);

            Task.Run(() =>
            {
                lock (_lock)
                {
                    File.AppendAllText(LogPath, line + Environment.NewLine);
                }
            });
        }

        public static void Info(string message) => Log("INFO", message);
        public static void Warn(string message) => Log("WARN", message);
        public static void Error(string message) => Log("ERROR", message);
    }
}
