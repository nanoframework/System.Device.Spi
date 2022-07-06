[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_System.Device.Spi&metric=alert_status)](https://sonarcloud.io/dashboard?id=nanoframework_System.Device.Spi) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_System.Device.Spi&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_System.Device.Spi) [![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![NuGet](https://img.shields.io/nuget/dt/nanoFramework.System.Device.Spi.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.System.Device.Spi/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

### Welcome to the .NET **nanoFramework** System.Device.Spi Library repository

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| System.Device.Spi | [![Build Status](https://dev.azure.com/nanoframework/System.Device.Spi/_apis/build/status/nanoframework.System.Device.Spi?repoName=nanoframework%2FSystem.Device.Spi&branchName=main)](https://dev.azure.com/nanoframework/System.Device.Spi/_build/latest?definitionId=72&repoName=nanoframework%2FSystem.Device.Spi&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.System.Device.Spi.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.System.Device.Spi/) |

## Usage

To create a SpiDevice, you need to follow this pattern:

```csharp
SpiDevice spiDevice;
SpiConnectionSettings connectionSettings;
// Note: the ChipSelect pin should be adjusted to your device, here 12
// You can adjust as well the bus, here 1 for SPI1
connectionSettings = new SpiConnectionSettings(1, 12);
spiDevice = SpiDevice.Create(connectionSettings);
```

If you'll be controlling the state of the Chip Select line, you need to pass `-1` to the second parameter of `SpiConnectionSettings(...)`. When specifying a GPIO number the driver takes care of this for you by setting the appropriate state for the signal during the SPI transactions.

### SpiConnectionSettings and SpiBusInfo

The `SpiConnectionSettings` contains the key elements needed to setup properly the hardware SPI device. Once created, those elements can't be adjusted.

```csharp
connectionSettings.ChipSelectLineActiveState = PinValue.High; // Default is active Low
connectionSettings.ClockFrequency = 1_000_000;
connectionSettings.DataFlow = DataFlow.LsbFirst; // Default is MSB
```

To get the default bus values, especially the bus min and max frequencies, the maximum number of ChipSelect, you can use `SpiBusInfo`.

```csharp
SpiBusInfo spiBusInfo = SpiDevice.GetBusInfo(1);
Debug.WriteLine($"{nameof(spiBusInfo.MaxClockFrequency)}: {spiBusInfo.MaxClockFrequency}");
Debug.WriteLine($"{nameof(spiBusInfo.MinClockFrequency)}: {spiBusInfo.MinClockFrequency}");
```

### Reading and Writing

You can read, write and do a full transfer. You have the opportunity to use either a `SpanByte`, either a `ushort` array. 

**Important**: in both cases, the data bit length will be automatically adjusted. So you can use both `SpanByte` and `ushort` array at the same time.

You can write a `SpanByte` like this:

```csharp
SpanByte writeBuffer = new byte[2] { 42, 84 };
spiDevice.Write(writeBuffer);
```

You can write a `ushort` array like this:

```csharp
ushort[] writeBuffer = new ushort[2] { 4200, 8432 };
spiDevice.Write(writeBuffer);
```

You can as well write a single byte:

```csharp
spiDevice.WriteByte(42);
```

Read operations are similar:

```csharp
SpanByte readBuffer = new byte[2];
// This will read 2 bytes
spiDevice.Read(readBuffer);
ushort[] readUshort = new ushort[4];
// This will read 4 ushort
spiDevice.Read(readUshort);
// read 1 byte
byte readMe = spiDevice.ReadByte();
```

For full transfer, you need to have 2 arrays of the same size and perform a full duplex transfer:

```csharp
SpanByte writeBuffer = new byte[4] { 0xAA, 0xBB, 0xCC, 0x42 };
SpanByte readBuffer = new byte[4];
spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
// Same for ushirt arrays:
ushort[] writeBuffer = new ushort[4] { 0xAABC, 0x00BB, 0xCC00, 0x4242 };
ushort[] readBuffer = new ushort[4];
spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
```

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The **nanoFramework** Class Libraries are licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behaviour in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
