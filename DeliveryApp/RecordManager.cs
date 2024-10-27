namespace EffectiveMobile {
  class RecordManager {
    private Dictionary<long, DeliveryRecord> _records;

    public RecordManager() {
      _records = new Dictionary<long, DeliveryRecord>();
    }

    public RecordManager(string data) : this() {
      AddRecord(data);
    }

    public RecordManager(string[] data) : this() {
      AddMultipleRecords(data);
    }

    public void AddMultipleRecords(StreamReader reader) {
      string ? line;
      while ((line = reader.ReadLine()) != null) {
        AddRecord(line);
      }
    }

    public void AddRecord(string data) {
      try {
        var record = new DeliveryRecord(data);
        if (_records.ContainsKey(record.GetID())) {
          Logger.Log(level: LogLevel.Warning,
                     message: $@"ID duplicate found: " + $@"[1]""{record.ToString(';')}""" +
                         $@"[2]""{data}""",
                     null);
        } else {
          _records.Add(record.GetID(), record);
        }
      } catch (Exception ex) {
        Logger.Log(level: LogLevel.Warning, message: "Can't add record.", ex: ex);
      }
    }

    public void AddMultipleRecords(string[] data) {
      foreach (string line in data) {
        AddRecord(line);
      }
    }

    public IEnumerable<DeliveryRecord> FilterRecords(string cityDistrict,
                                                     DateTime firstDeliveryDateTime) {
      var districtOrders =
          _records.Values.Where(r => r.GetCityDistrict().ToLower().Equals(cityDistrict.ToLower()))
              .OrderBy(r => r.GetDeliveryDateTime());

      if (districtOrders.Any()) {
        DateTime actualFirstDeliveryDateTime =
            districtOrders.Where(r => r.GetDeliveryDateTime() >= firstDeliveryDateTime)
                .First()
                .GetDeliveryDateTime();

        var timeDistrictOrders =
            districtOrders
                .Where(d => d.GetDeliveryDateTime() >= actualFirstDeliveryDateTime &&
                            d.GetDeliveryDateTime() <= actualFirstDeliveryDateTime.AddMinutes(30))
                .OrderBy(d => d.GetDeliveryDateTime());

        return timeDistrictOrders;
      } else {
        Logger.Log(level: LogLevel.Notice,
                   $@"There is no district ""{cityDistrict}"" in input file.");
        return Enumerable.Empty<DeliveryRecord>();
      }
    }
  }
}
