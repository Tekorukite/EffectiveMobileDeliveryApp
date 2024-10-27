namespace EffectiveMobile {
  static class Constants {
    public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
  }

  static class Utils {
    public static void DisplayHelp() {
      Console.WriteLine(
          @"EffectiveMobile DeliveryApp usage:
            DeliveryApp [--help] [--option1 value1] [--option2 value2] ...

            Options:
              --cityDistrict : City district name
              --firstDeliveryDateTime : Date and time before the first delivery
                                        in format ""yyyy-MM-dd HH:mm:ss""
              --deliveryLog : Path to log file
              --deliveryOrder : Path to output file
              --help : Show this message


            Defaults:
              --firstDeliveryDateTime ""DateTime.Now""
              --deliverLog ""./DeliveryApp.log""
              --deliveryOrder ""./DeliveryOrder.txt""

            Examples:
              DeliveryApp --cityDistrict Bronx
              DeliveryApp --cityDistrict ""Glen Cove"" --firstDeliveryDateTime ""2024-12-31 11:22:33""
              DeliveryApp --cityDistrict Bronx --deliverOrder ""~/orders/delivery_app_order.txt""

            ");
    }

    public static void WriteOutput(IEnumerable<DeliveryRecord> deliveryRecords,
                                   ConsoleArguments consoleArguments) {
      using (StreamWriter writer = new StreamWriter(consoleArguments.DeliveryOrder, false)) {
        if (!deliveryRecords.Any()) {
          Console.WriteLine(
              $"There is no orders in {consoleArguments.CityDistrict} " +
              $"from {consoleArguments.FirstDeliveryDateTime.ToString()} " +
              $"to {consoleArguments.FirstDeliveryDateTime.AddMinutes(30).ToString()}");
        } else {
          var actualFirstDeliverDateTime = deliveryRecords.First().GetDeliveryDateTime();

          Console.WriteLine(
              $"There are {deliveryRecords.Count()} orders in {consoleArguments.CityDistrict} " +
              $"from {actualFirstDeliverDateTime.ToString(Constants.DateTimeFormat)} " +
              $"(first actual delivery from {consoleArguments.FirstDeliveryDateTime.ToString(Constants.DateTimeFormat)}) " +
              $"to {actualFirstDeliverDateTime.AddMinutes(30).ToString(Constants.DateTimeFormat)}.\n" +
              $@"Output saved to file: ""{consoleArguments.DeliveryOrder}""");

          writer.WriteLine(
              $"There are {deliveryRecords.Count()} orders in {consoleArguments.CityDistrict} " +
              $"from {actualFirstDeliverDateTime.ToString(Constants.DateTimeFormat)} " +
              $"(first actual delivery from {consoleArguments.FirstDeliveryDateTime.ToString(Constants.DateTimeFormat)}) " +
              $"to {actualFirstDeliverDateTime.AddMinutes(30).ToString(Constants.DateTimeFormat)}");

          foreach (var record in deliveryRecords) {
            writer.WriteLine(record.ToString(';'));
          }
        }
      }
    }

    public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args) {
      Exception ex = (Exception)args.ExceptionObject;
      Logger.Log(level: LogLevel.Fatal, $@"Program terminating = {args.IsTerminating}", ex);
    }
  }
}
