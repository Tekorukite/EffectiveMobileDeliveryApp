namespace EffectiveMobile
{
    public class DeliveryRecord
    {
        internal long _id;
        internal double _weight;
        internal string _cityDistrict = "";
        internal DateTime _deliveryDateTime;

        public DeliveryRecord(string data)
        {
            var splitData = data.Split(';');
            if (splitData.Length != 4)
            {
                throw new InvalidDataException($@"String ""{data}"" can't be parsed.");
            }

            if (!long.TryParse(splitData[0], out _id) || _id < 0)
            {
                throw new InvalidCastException($@"String ""{splitData[0]}"" can't be parsed to long");
            }

            if (!double.TryParse(splitData[1], out _weight) || _weight < 0.0)
            {
                throw new InvalidCastException($@"String ""{splitData[1]}"" can't be parsed to double");
            }

            if (String.IsNullOrEmpty(splitData[2]) || String.IsNullOrWhiteSpace(splitData[2]))
            {
                throw new InvalidDataException($@"CityDistrict field can't be empty or whitespace");
            }
            else
            {
                _cityDistrict = splitData[2];
            }

            if (
                !DateTime.TryParseExact(
                    splitData[3],
                    Constants.DateTimeFormat,
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out _deliveryDateTime
                )
            )
            {
                throw new InvalidCastException(
                    $@"String ""{splitData[3]}"" can't be parsed to DateTime or invalid format"
                );
            }
        }

        public long GetID()
        {
            return _id;
        }

        public string GetCityDistrict()
        {
            return _cityDistrict;
        }

        public DateTime GetDeliveryDateTime()
        {
            return _deliveryDateTime;
        }

        public string ToString(char delimeter)
        {
            return $"{_id.ToString(), -10} {delimeter} "
                + $"{_weight.ToString(), -7} {delimeter} "
                + $"{_cityDistrict, -20} {delimeter} "
                + $"{_deliveryDateTime.ToString(Constants.DateTimeFormat)}";
        }

        public string ToJsonString()
        {
            var json = new
            {
                Id = _id,
                Weight = _weight,
                CityDistrict = _cityDistrict,
                DeliveryDateTime = _deliveryDateTime,
            };
            return System.Text.Json.JsonSerializer.Serialize(json);
        }
    }
}
