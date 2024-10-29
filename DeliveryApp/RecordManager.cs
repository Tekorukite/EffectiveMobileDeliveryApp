namespace EffectiveMobile
{
    public class RecordManager
    {
        internal Dictionary<ulong, DeliveryRecord> _records;

        public RecordManager()
        {
            _records = new Dictionary<ulong, DeliveryRecord>();
        }

        public void AddMultipleRecords(StreamReader reader)
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                try
                {
                    AddRecord(line);
                }
                catch (InvalidDataException ex)
                {
                    Logger.Log(level: LogLevel.Warning, message: ex.Message, ex: ex);
                }
            }
        }

        public void AddRecord(string data)
        {
            try
            {
                var record = new DeliveryRecord(data);
                if (_records.ContainsKey(record.GetID()))
                {
                    throw new InvalidDataException(
                        $@"ID duplicate found: "
                            + $@"[1]""{record.ToString(';')}"""
                            + $@"[2]""{data}"""
                    );
                }
                else
                {
                    _records.Add(record.GetID(), record);
                }
            }
            catch (Exception ex) when (!ex.GetType().Equals(typeof(InvalidDataException)))
            {
                Logger.Log(level: LogLevel.Error, message: "Can't add record.", ex: ex);
            }
        }

        public void AddMultipleRecords(string[] data)
        {
            foreach (string line in data)
            {
                try
                {
                    AddRecord(line);
                }
                catch (InvalidDataException ex)
                {
                    Logger.Log(level: LogLevel.Warning, message: ex.Message, ex: ex);
                }
            }
        }

        public IEnumerable<DeliveryRecord> FilterRecords(
            string cityDistrict,
            DateTime firstDeliveryDateTime
        )
        {
            var districtOrders = _records
                .Values.Where(r => r.GetCityDistrict().ToLower().Equals(cityDistrict.ToLower()))
                .OrderBy(r => r.GetDeliveryDateTime());

            if (districtOrders.Any())
            {
                DateTime actualFirstDeliveryDateTime = districtOrders
                    .Where(r => r.GetDeliveryDateTime() >= firstDeliveryDateTime)
                    .First()
                    .GetDeliveryDateTime();

                var timeDistrictOrders = districtOrders
                    .Where(d =>
                        d.GetDeliveryDateTime() >= actualFirstDeliveryDateTime
                        && d.GetDeliveryDateTime() <= actualFirstDeliveryDateTime.AddMinutes(30)
                    )
                    .OrderBy(d => d.GetDeliveryDateTime());

                return timeDistrictOrders;
            }
            else
            {
                Logger.Log(
                    level: LogLevel.Notice,
                    $@"There is no district ""{cityDistrict}"" in input file."
                );
                return Enumerable.Empty<DeliveryRecord>();
            }
        }
    }
}
