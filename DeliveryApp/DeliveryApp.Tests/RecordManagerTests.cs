namespace EffectiveMobile.Tests.RecordManager
{
    public class RecordManagerTest
    {
        [Fact]
        public void RecordManagerConstructorValidEmpty()
        {
            var manager = new EffectiveMobile.RecordManager();
            Assert.Empty(manager._records);
        }

        [Fact]
        public void RecordManagerAddRecordValid()
        {
            var manager = new EffectiveMobile.RecordManager();
            manager.AddRecord("1;2;district;2000-01-01 00:00:00");
            Assert.Single(manager._records);
            Assert.True(manager._records.ContainsKey(1));

            manager.AddRecord("2;3;district;2000-01-01 01:01:01");
            Assert.Equal(2, manager._records.Count);
            Assert.True(manager._records.ContainsKey(2));

            manager.AddRecord("3;4;district;wrong date");
            Assert.Equal(2, manager._records.Count);
            Assert.False(manager._records.ContainsKey(3));
        }

        [Fact]
        public void RecordManagerInvalidId()
        {
            var manager = new EffectiveMobile.RecordManager();
            manager.AddRecord("12345;2;district;2000-01-01 00:00:00");
            Assert.Throws<InvalidDataException>(() => manager.AddRecord("12345;3;another;2000-02-02 00:00:00"));
        }
    }
}
