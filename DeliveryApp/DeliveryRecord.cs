namespace EffectiveMobile {
  class DeliveryRecord {
    private long _id;
    private double _weight;
    private string _cityDistrict = "";
    private DateTime _deliveryDateTime;

    public DeliveryRecord(string data) {
      var splitData = data.Split(';');
      if (splitData.Length != 4) {
        throw new InvalidDataException($@"String ""{data}"" can't be parsed.");
      }

      if (!long.TryParse(splitData[0], out _id)) {
        throw new InvalidCastException($@"String ""{splitData[0]}"" can't be parsed to long");
      }

      if (!double.TryParse(splitData[1], out _weight)) {
        throw new InvalidCastException($@"String ""{splitData[1]}"" can't be parsed to double");
      }

      if (String.IsNullOrEmpty(splitData[2]) || String.IsNullOrWhiteSpace(splitData[2])) {
        throw new InvalidDataException($@"CityDistrict field can't be empty or whitespace");
      } else {
        _cityDistrict = splitData[2];
      }

      if (!DateTime.TryParseExact(splitData[3], Constants.DateTimeFormat, null,
                                  System.Globalization.DateTimeStyles.None,
                                  out _deliveryDateTime)) {
        throw new InvalidCastException(
            $@"String ""{splitData[3]}"" can't be parsed to DateTime or invalid format");
      }
    }

    public long GetID() {
      return _id;
    }

    public string GetCityDistrict() {
      return _cityDistrict;
    }

    public DateTime GetDeliveryDateTime() {
      return _deliveryDateTime;
    }

    public string ToString(char delimeter) {
      return $"{_id.ToString()}{delimeter}" + $"{_weight.ToString()}{delimeter}" +
             $"{_cityDistrict}{delimeter}" +
             $"{_deliveryDateTime.ToString(Constants.DateTimeFormat)}";
    }
  }
}