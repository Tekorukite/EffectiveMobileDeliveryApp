# EffectiveMobileDeliveryApp

DeliveryApp is an application for a delivery service that sorts orders based on
their time of request within a specific area of a city.

## Installation

- Open the terminal in the root directory of the repository.
- Run the following command: `dotnet publish ./DeliveryApp/DeliveryApp.csproj`
- Executable file will be located at
  `./DeliveryApp/bin/Release/net8.0/[platform]/publish/`

## Environment variables

- EM_DELIVERY_DATA: The path to the input data file.
- EM_DELIVERY_LOG: The path to the file where program logs will be saved.
- EM_DELIVERY_ORDER: The path of the file that will contain filtered results.

To define this variables run in terminal:

- Windows:
  - `set VARNAME=VALUE`
- Linux/MacOS:
  - `export VARNAME=VALUE`

## Usage

The program should be executed with command-line arguments.

- Required parameters:
  - `--cityDistrict` | `-d` - name of the district in the city (case
    insensitive). If there is a space within the name, name must be enclosed in
    quotation marks.
- Optional parameters:
  - `--inputData` | `-i` - path to the input data file. Data columns must be
    separated with semicolon. Examples are shown in `./data/DeliveryData.csv`
    and `./data/DeliveryDataBig.csv`
  - `--firstDeliveryDateTime` | `-f` - time of the first delivery. If not
    specified, uses DateTime.Now. If there are no orders within this district of
    the city during this time period, the nearest order will be found after this
    time. Must be enclosed in quotation marks. Expected format: "yyyy-MM-dd
    HH:mm:ss".
  - `--deliveryLog` | `-l` - path to the file where program logs will be saved.
  - `--deliveryOrder` | `-o` - path to the file that will contain filtered
    results.

## Examples:

> DeliveryApp --cityDistrict Bronx

> DeliveryApp --cityDistrict Glen Cove" --firstDeliveryDateTime 2024-12-31
> 11:22:33"

> DeliveryApp --cityDistrict Bronx --deliverOrder
> "~/orders/delivery_app_order.txt"

> DeliveryApp -i "/home/user/data/InputData.csv" -d Bronx -f "2024-11-02
> 01:23:45" -l "~/.log/Delivery.log" -o "~/DeliveryOrder.txt"
