namespace EffectiveMobile
{
  enum LogLevel
  {
    Debug,
    Info,
    Notice,
    Warning,
    Error,
    Fatal,
  }

  static class Logger
  {
    private static string _filePath = "./DeliveryApp.log";
    private static LogLevel _minLogLevel = LogLevel.Debug;

    static Logger() { }

    public static void Log(LogLevel level, string message, Exception? ex = null)
    {
      if (level >= _minLogLevel)
      {
        using (StreamWriter writer = new StreamWriter(_filePath, true))
        {
          writer.WriteLine(
              $"{DateTime.Now} | {level.ToString().ToUpper(),-7} | {message}"
                  + (ex != null ? $"\n[{ex.ToString()}]" : String.Empty)
          );
        }
      }
    }

    public static void MinLogLevel(LogLevel level)
    {
#if DEBUG
      _minLogLevel = LogLevel.Debug;
#else
            _minLogLevel = level;
#endif
    }

    public static void FilePath(string filePath)
    {
      _filePath = filePath;
    }
  }
}
