namespace EffectiveMobile
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(Utils.UnhandledExceptionHandler);

            var appArguments = new AppArguments();
            Logger.FilePath(appArguments.DeliveryLog);
            Logger.MinLogLevel(LogLevel.Warning);

            var recordManager = new RecordManager();

            try
            {
                using (StreamReader reader = new StreamReader(AppArguments.InputDataFileName))
                {
                    recordManager.AddMultipleRecords(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    level: LogLevel.Error,
                    message: $@"No such file or not permitted: ""{AppArguments.InputDataFileName}""",
                    ex: ex
                );
                Console.WriteLine($@"No such file or not permitted: ""{AppArguments.InputDataFileName}""");
                Environment.Exit(0);
            }

            var filteredData = recordManager.FilterRecords(
                cityDistrict: appArguments.CityDistrict,
                firstDeliveryDateTime: appArguments.FirstDeliveryDateTime
            );

            Utils.WriteOutput(deliveryRecords: filteredData, appArguments: appArguments);
        }
    }
}
