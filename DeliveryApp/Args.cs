namespace EffectiveMobile {
  struct ConsoleArguments {
    public string CityDistrict;
    public DateTime FirstDeliveryDateTime = DateTime.Now;
    public string DeliveryLog = @"./DeliveryApp.log";
    public string DeliveryOrder = @"./DeliveryOrder.txt";

    public ConsoleArguments() {
      var args = Environment.GetCommandLineArgs();

      if (args.Length < 3 || args[1] == "--help") {
        Utils.DisplayHelp();
        Environment.Exit(0);
      }

      for (int i = 1; i < args.Length - 1; ++i) {
        switch (args[i]) {
          case "--cityDistrict": {
            CityDistrict = args[i + 1];
            break;
          }

          case "--firstDeliveryDateTime": {
            if (!DateTime.TryParseExact(args[i + 1], Constants.DateTimeFormat, null,
                                        System.Globalization.DateTimeStyles.None,
                                        out FirstDeliveryDateTime)) {
              Utils.DisplayHelp();
              Environment.Exit(0);
            }
            break;
          }

          case "--deliveryLog": {
            DeliveryLog = args[i + 1];
            break;
          }

          case "--deliveryOrder": {
            DeliveryOrder = args[i + 1];
            break;
          }
        }
      }

      if (String.IsNullOrEmpty(CityDistrict) || String.IsNullOrWhiteSpace(CityDistrict)) {
        Utils.DisplayHelp();
        Environment.Exit(0);
      }
    }
  }
}
