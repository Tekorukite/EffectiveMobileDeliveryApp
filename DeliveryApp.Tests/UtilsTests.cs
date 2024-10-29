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

            var appArgs = new EffectiveMobile.AppArguments();
            appArgs.Parse(args);

            Assert.Equal(Path.GetFullPath("./data.csv"), appArgs.InputDataFileName);
            Assert.Equal(Path.GetFullPath("./output.log"), appArgs.DeliveryLog);
            Assert.Equal(Path.GetFullPath("./output.txt"), appArgs.DeliveryOrder);
            Assert.Equal(Path.GetFullPath("./output.json"), appArgs.DeliveryOrderJson);
            Assert.Equal("Bronx", appArgs.CityDistrict);
            Assert.Equal(DateTime.Parse("2000-01-01 00:00:00"), appArgs.FirstDeliveryDateTime);
        }

        [Fact]
        public void AppArgumentsDistrictOnly()
        {
            var dateTimeNow = DateTime.Now;
            var appArgs = new EffectiveMobile.AppArguments();
            appArgs.Parse(["--cityDistrict", "Arbat"]);
            string currentDir = EffectiveMobile.AppArguments._currentDirectory;

            Assert.Equal(
                Path.GetFullPath(currentDir + "/DeliveryData.csv"),
                appArgs.InputDataFileName
            );
            Assert.Equal(
                Path.GetFullPath(currentDir + "/DeliveryOrder.txt"),
                appArgs.DeliveryOrder
            );
            Assert.Equal(
                Path.GetFullPath(currentDir + "/DeliveryOrder.json"),
                appArgs.DeliveryOrderJson
            );
            Assert.Equal(Path.GetFullPath(currentDir + "/DeliveryApp.log"), appArgs.DeliveryLog);
            Assert.True((appArgs.FirstDeliveryDateTime - dateTimeNow).TotalSeconds < 1);
        }

        [Fact]
        public void AppArgumentsEmptyDistrict()
        {
            var appArgs = new EffectiveMobile.AppArguments();
            Assert.Throws<InvalidDataException>(() => appArgs.Parse(["-i", "input.data"]));
        }

        [Fact]
        public void AppArgumentsInvalidDate()
        {
            var appArgs = new EffectiveMobile.AppArguments();
            Assert.Throws<InvalidCastException>(
                () => appArgs.Parse(["-d", "Bronx", "-f", "2000/01/01 00:00:02"])
            );
        }

        [Fact]
        public void AppArgumentsEnvVarsOnly()
        {
            Environment.SetEnvironmentVariable("EM_DELIVERY_DATA", "/etc/delivery.csv");
            Environment.SetEnvironmentVariable("EM_DELIVERY_LOG", "/var/log/delivery.log");
            Environment.SetEnvironmentVariable("EM_DELIVERY_ORDER", "/home/delivery/order.txt");

            string[] args = ["-d", "Bronx"];

            var appArgs = new EffectiveMobile.AppArguments();
            appArgs.Parse(args);

            Assert.Equal(Path.GetFullPath("/etc/delivery.csv"), appArgs.InputDataFileName);
            Assert.Equal(Path.GetFullPath("/var/log/delivery.log"), appArgs.DeliveryLog);
            Assert.Equal(Path.GetFullPath("/home/delivery/order.txt"), appArgs.DeliveryOrder);
            Assert.Equal(Path.GetFullPath("/home/delivery/order.json"), appArgs.DeliveryOrderJson);
            Assert.Equal("Bronx", appArgs.CityDistrict);

            Environment.SetEnvironmentVariable("EM_DELIVERY_DATA", "");
            Environment.SetEnvironmentVariable("EM_DELIVERY_LOG", "");
            Environment.SetEnvironmentVariable("EM_DELIVERY_ORDER", "");
        }
    }
}
