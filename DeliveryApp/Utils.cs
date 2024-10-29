namespace EffectiveMobile
{
    struct AppArguments
    {
        internal static string _currentDirectory = Environment.CurrentDirectory;

        public string InputDataFileName;

        public string CityDistrict;
        public DateTime FirstDeliveryDateTime;
        public string DeliveryLog;
        public string DeliveryOrder;
        public string DeliveryOrderJson;

        public AppArguments()
        {
            InputDataFileName = Path.GetFullPath(
                Environment.GetEnvironmentVariable("EM_DELIVERY_DATA")
                    ?? _currentDirectory + "/DeliveryData.csv"
            );

            FirstDeliveryDateTime = DateTime.Now;

            DeliveryLog = Path.GetFullPath(
                Environment.GetEnvironmentVariable("EM_DELIVERY_LOG")
                    ?? _currentDirectory + "/DeliveryApp.log"
            );

            CityDistrict = String.Empty;

            DeliveryOrder = Path.GetFullPath(
                Environment.GetEnvironmentVariable("EM_DELIVERY_ORDER")
                    ?? _currentDirectory + "/DeliveryOrder.txt"
            );

            DeliveryOrderJson = Path.GetFullPath(
                Path.GetDirectoryName(DeliveryOrder)
                    + "/"
                    + Path.GetFileNameWithoutExtension(DeliveryOrder)
                    + ".json"
            );
        }

        public void Parse(string[] args)
        {
            for (int i = 0; i < args.Length - 1; ++i)
            {
                switch (args[i])
                {
                    case "-i":
                    case "--inputData":
                    {
                        InputDataFileName = Path.GetFullPath(args[i + 1]);
                        break;
                    }
                    case "-d":
                    case "--cityDistrict":
                    {
                        CityDistrict = args[i + 1];
                        break;
                    }
                    case "-f":
                    case "--firstDeliveryDateTime":
                    {
                        if (
                            !DateTime.TryParseExact(
                                args[i + 1],
                                Constants.DateTimeFormat,
                                null,
                                System.Globalization.DateTimeStyles.None,
                                out FirstDeliveryDateTime
                            )
                        )
                        {
                            throw new InvalidCastException(
                                $@"DateTime String can't be parsed. String: ""{args[i + 1]}"" Expected format: ""{Constants.DateTimeFormat}"""
                            );
                        }
                        break;
                    }
                    case "-l":
                    case "--deliveryLog":
                    {
                        DeliveryLog = Path.GetFullPath(args[i + 1]);
                        break;
                    }
                    case "-o":
                    case "--deliveryOrder":
                    {
                        DeliveryOrder = Path.GetFullPath(args[i + 1]);
                        break;
                    }
                }
            }

            DeliveryOrderJson = Path.GetFullPath(
                Path.GetDirectoryName(DeliveryOrder)
                    + "/"
                    + Path.GetFileNameWithoutExtension(DeliveryOrder)
                    + ".json"
            );

            if (String.IsNullOrEmpty(CityDistrict) || String.IsNullOrWhiteSpace(CityDistrict))
            {
                throw new InvalidDataException("City district can't be empty.");
            }
        }
    }

    static class Utils
    {
        public static void DisplayHelp()
        {
            Console.WriteLine(Constants.HelpMessage);
        }

        public static void WriteOutput(
            IEnumerable<DeliveryRecord> deliveryRecords,
            AppArguments appArguments
        )
        {
            using (StreamWriter stringWriter = new StreamWriter(appArguments.DeliveryOrder, false))
            using (
                StreamWriter jsonWriter = new StreamWriter(appArguments.DeliveryOrderJson, false)
            )
            {
                if (!deliveryRecords.Any())
                {
                    Console.WriteLine(
                        SummaryMessage(
                            recordsCount: deliveryRecords.Count(),
                            actualFirstDeliveryDateTime: DateTime.MinValue,
                            appArguments: appArguments
                        )
                    );
                }
                else
                {
                    var actualFirstDeliverDateTime = deliveryRecords.First().GetDeliveryDateTime();

                    Console.WriteLine(
                        SummaryMessage(
                            recordsCount: deliveryRecords.Count(),
                            actualFirstDeliveryDateTime: actualFirstDeliverDateTime,
                            appArguments: appArguments
                        )
                    );
                    Console.WriteLine($@"Output saved to files:");
                    Console.WriteLine($@"  {appArguments.DeliveryOrder}");
                    Console.WriteLine($@"  {appArguments.DeliveryOrderJson}");

                    stringWriter.WriteLine(
                        SummaryMessage(
                            recordsCount: deliveryRecords.Count(),
                            actualFirstDeliveryDateTime: actualFirstDeliverDateTime,
                            appArguments: appArguments
                        )
                    );

                    foreach (var record in deliveryRecords)
                    {
                        stringWriter.WriteLine(record.ToString('|'));
                        jsonWriter.WriteLine(record.ToJsonString());
                    }
                }
            }
        }

        private static string SummaryMessage(
            int recordsCount,
            DateTime actualFirstDeliveryDateTime,
            AppArguments appArguments
        )
        {
            var sb = new System.Text.StringBuilder();
            string firstDelivery = appArguments.FirstDeliveryDateTime.ToString(
                Constants.DateTimeFormat
            );

            if (recordsCount == 0)
            {
                sb.AppendLine(
                    $"There are no orders in {appArguments.CityDistrict} from {firstDelivery}."
                );
                sb.AppendLine(
                    $@"Output files ""{appArguments.DeliveryOrder}"" and ""{appArguments.DeliveryOrderJson}"" have remained unchanged."
                );
                return sb.ToString();
            }

            string actualFirstDelivery = actualFirstDeliveryDateTime.ToString(
                Constants.DateTimeFormat
            );
            string actualEndDelivery = actualFirstDeliveryDateTime
                .AddMinutes(Constants.DeliverySpanMinutes)
                .ToString(Constants.DateTimeFormat);

            if (actualFirstDeliveryDateTime > appArguments.FirstDeliveryDateTime)
            {
                sb.AppendLine(
                    $"There are no orders in {appArguments.CityDistrict} from {firstDelivery} to {actualFirstDelivery}."
                );

                sb.AppendLine("-------------------------");
            }
            sb.AppendLine(
                $"There are {recordsCount} orders in {appArguments.CityDistrict} "
                    + $"from {actualFirstDelivery} to {actualEndDelivery}."
            );

            return sb.ToString();
        }

        public static void UnhandledExceptionHandler(
            object sender,
            UnhandledExceptionEventArgs args
        )
        {
            Exception ex = (Exception)args.ExceptionObject;
            Logger.Log(level: LogLevel.Fatal, $@"Program terminating = {args.IsTerminating}", ex);
        }
    }
}
