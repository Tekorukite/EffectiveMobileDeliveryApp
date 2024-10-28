namespace EffectiveMobile.Tests.Utils
{
    public class AppArgumentsTest
    {
        [Fact]
        public void AppArgumentsMockCommandLineArgs()
        {
            string[] args =
            [
                "-i",
                "./data.csv",
                "-l",
                "./output.log",
                "-o",
                "./output.txt",
                "-d",
                "Bronx",
                "-f",
                "2000-01-01 00:00:00",
            ];

            var appArgs = new EffectiveMobile.AppArguments(args: args);

            Assert.Equal("./data.csv", appArgs.InputDataFileName);
            Assert.Equal("./output.log", appArgs.DeliveryLog);
            Assert.Equal("./output.txt", appArgs.DeliveryOrder);
            Assert.Equal("./output.json", appArgs.DeliveryOrderJson);
            Assert.Equal("Bronx", appArgs.CityDistrict);
            Assert.Equal(DateTime.Parse("2000-01-01 00:00:00"), appArgs.FirstDeliveryDateTime);
        }

        [Fact]
        public void AppArgumentsDistrictOnly()
        {
            var dateTimeNow = DateTime.Now;
            var appArgs = new EffectiveMobile.AppArguments(["--cityDistrict", "Arbat"]);
            string currentDir = EffectiveMobile.AppArguments._currentDirectory;

            Assert.Equal(currentDir + "/DeliveryData.csv", appArgs.InputDataFileName);
            Assert.Equal(currentDir + "/DeliveryOrder.txt", appArgs.DeliveryOrder);
            Assert.Equal(currentDir + "/DeliveryOrder.json", appArgs.DeliveryOrderJson);
            Assert.Equal(currentDir + "/DeliveryApp.log", appArgs.DeliveryLog);
            Assert.True((appArgs.FirstDeliveryDateTime - dateTimeNow).TotalSeconds < 1);
        }

        [Fact]
        public void AppArgumentsEnvVarsOnly()
        {
            Environment.SetEnvironmentVariable("EM_DELIVERY_DATA", "/etc/delivery.csv");
            Environment.SetEnvironmentVariable("EM_DELIVERY_LOG", "/var/log/delivery.log");
            Environment.SetEnvironmentVariable("EM_DELIVERY_ORDER", "/home/delivery/order.txt");

            string[] args = ["-d", "Bronx"];

            var appArgs = new EffectiveMobile.AppArguments(args: args);

            Assert.Equal("/etc/delivery.csv", appArgs.InputDataFileName);
            Assert.Equal("/var/log/delivery.log", appArgs.DeliveryLog);
            Assert.Equal("/home/delivery/order.txt", appArgs.DeliveryOrder);
            Assert.Equal("/home/delivery/order.json", appArgs.DeliveryOrderJson);
            Assert.Equal("Bronx", appArgs.CityDistrict);

            Environment.SetEnvironmentVariable("EM_DELIVERY_DATA", "");
            Environment.SetEnvironmentVariable("EM_DELIVERY_LOG", "");
            Environment.SetEnvironmentVariable("EM_DELIVERY_ORDER", "");
        }
    }
}
