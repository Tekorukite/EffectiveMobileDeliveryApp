namespace EffectiveMobile.Tests.DeliveryRecord
{
    public class DeliveryRecordTest
    {
        [Theory]
        [InlineData((long)123, (double)3.333, "SomeDistrict", "2024-10-31 12:34:56")]
        [InlineData((long)2, (double)1.00, "Another District", "2024-10-13 22:22:22")]
        [InlineData((long)1000000000, (double)0.01, "And-Another-One@Moscow", "2000-01-01 00:00:00")]
        public void ConstructorValid(long id, double weight, string district, string dateString)
        {
            var record = new EffectiveMobile.DeliveryRecord($"00{id};  {weight} ;{district};{dateString}");
            Assert.Equal(id, record._id);
            Assert.Equal(weight, record._weight);
            Assert.Equal(district, record._cityDistrict);
            Assert.Equal(DateTime.Parse(dateString), record._deliveryDateTime);
        }

        [Theory]
        [InlineData("1;2;3")]
        [InlineData("1/2/district/2000-01-01 00:00:00")]
        [InlineData("1;2;district;2000-01-01 00:00:00;5")]
        public void ConstructorInvalidFieldsCount(string data)
        {
            Assert.Throws<InvalidDataException>(() => new EffectiveMobile.DeliveryRecord(data));
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("a")]
        [InlineData("123.2")]
        [InlineData("")]
        public void ConstructorInvalidId(string id)
        {
            Assert.Throws<InvalidCastException>(
                () => new EffectiveMobile.DeliveryRecord($"{id};1;district;2000-01-01 00:00:00")
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("-1.2")]
        [InlineData(" ")]
        public void ConstructorInvalidWeight(string weight)
        {
            Assert.Throws<InvalidCastException>(
                () => new EffectiveMobile.DeliveryRecord($"1;{weight};district;2000-01-01 00:00:00")
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("\t")]
        public void ConstructorInvalidCityDistrict(string district)
        {
            Assert.Throws<InvalidDataException>(
                () => new EffectiveMobile.DeliveryRecord($"1;1;{district};2000-01-01 00:00:00")
            );
        }

        [Theory]
        [InlineData("2000/01/01 00:00:00")]
        [InlineData("2000-01-01 01-01-01")]
        [InlineData("10-31 02:02:02")]
        [InlineData("2000/13/13 03:03:03")]
        [InlineData("2000/02/29 04:04:04")]
        [InlineData("2000-01-01 05:05")]
        [InlineData("abc")]
        [InlineData("2000.02.02 00:00:00")]
        [InlineData("1")]
        public void ConstructorInvalidDateTime(string dateString)
        {
            Assert.Throws<InvalidCastException>(
                () => new EffectiveMobile.DeliveryRecord($"1;1.1;district;{dateString}")
            );
        }
    }
}
