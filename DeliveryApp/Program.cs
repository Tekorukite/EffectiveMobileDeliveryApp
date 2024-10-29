namespace EffectiveMobile
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "--help" || args[0] == "-h" || args.Length < 2)
            {
                Utils.DisplayHelp();
                Environment.Exit(0);
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(
                Utils.UnhandledExceptionHandler
            );

            AppArguments appArguments = new AppArguments();

            try
            {
                appArguments.Parse(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                Utils.DisplayHelp();
                Environment.Exit(0);
            }

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
