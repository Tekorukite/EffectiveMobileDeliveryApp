namespace EffectiveMobile
{
    static class Constants
    {
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const int DeliverySpanMinutes = 30;
        public const char DataDelimeter = ';';
        public const string HelpMessage =
            @"EffectiveMobile DeliveryApp usage:
DeliveryApp [--help] [--option1 value1] [--option2 value2] ...

Options:
    -i, --inputData                 Path to input data file.
                                    Can be specified via env variable EM_DELIVERY_DATA
    -d, --cityDistrict              City district name.
    -f, --firstDeliveryDateTime     Date and time before the first delivery
                                    in format ""yyyy-MM-dd HH:mm:ss"".
    -l, --deliveryLog               Path to log file. 
                                    Can be specified via env variable EM_DELIVERY_LOG
    -o, --deliveryOrder             Path to output file.
                                    Can be specified via env variable EM_DELIVERY_ORDER
    -h, --help                      Show this message.


Defaults:
    --firstDeliveryDateTime ""DateTime.Now""
    --deliverLog ""./DeliveryApp.log""
    --deliveryOrder ""./DeliveryOrder.txt""

Examples:
    DeliveryApp --cityDistrict Bronx
    DeliveryApp --cityDistrict ""Glen Cove"" --firstDeliveryDateTime ""2024-12-31 11:22:33""
    DeliveryApp --cityDistrict Bronx --deliverOrder ""~/orders/delivery_app_order.txt""
    DeliveryApp -i ""/home/user/data/InputData.csv"" -d Bronx -f ""2024-11-02 01:23:45"" -l ""~/.log/Delivery.log"" -o ""~/DeliveryOrder.txt""";
    }
}
