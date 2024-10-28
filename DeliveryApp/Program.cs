namespace EffectiveMobile
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(
                Utils.UnhandledExceptionHandler
            );

            var appArguments = new AppArguments(args: args);
            Logger.FilePath(filePath: appArguments.DeliveryLog);
            Logger.MinLogLevel(level: LogLevel.Warning);

            var recordManager = new RecordManager();

            try
            {
                using (StreamReader reader = new StreamReader(appArguments.InputDataFileName))
                {
                    recordManager.AddMultipleRecords(reader: reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    level: LogLevel.Error,
                    message: $@"No such file or not permitted: ""{appArguments.InputDataFileName}""",
                    ex: ex
                );
                Console.WriteLine(
                    $@"No such file or not permitted: ""{appArguments.InputDataFileName}"""
                );
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
