namespace EffectiveMobile {
  class Program {
    static void Main(string[] args) {
      AppDomain currentDomain = AppDomain.CurrentDomain;
      currentDomain.UnhandledException +=
          new UnhandledExceptionEventHandler(Utils.UnhandledExceptionHandler);

      string inputDataFileName =
          Environment.GetEnvironmentVariable("EM_DELIVERY_DATA_FILENAME") ?? "./DeliveryData.csv";
      var consoleArguments = new ConsoleArguments();
      Logger.FilePath(consoleArguments.DeliveryLog);
      Logger.MinLogLevel(LogLevel.Warning);

      var recordManager = new RecordManager();

      try {
        using (StreamReader reader = new StreamReader(inputDataFileName)) {
          recordManager.AddMultipleRecords(reader);
        }
      } catch (Exception ex) {
        Logger.Log(level: LogLevel.Error,
                   $@"No such file or not permitted: ""{inputDataFileName}""", ex);
        Console.WriteLine($@"No such file or not permitted: ""{inputDataFileName}""");
        Environment.Exit(0);
      }

      var filteredData = recordManager.FilterRecords(
          cityDistrict: consoleArguments.CityDistrict,
          firstDeliveryDateTime: consoleArguments.FirstDeliveryDateTime);

      Utils.WriteOutput(deliveryRecords: filteredData, consoleArguments: consoleArguments);
    }
  }
}
