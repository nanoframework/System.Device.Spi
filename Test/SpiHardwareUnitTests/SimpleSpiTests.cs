//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TestFramework;
using System;
using System.Device.Gpio;
using System.Device.Spi;
using System.Diagnostics;

namespace SpiHardwareUnitTests
{
    [TestClass]
    public class SimpleSpiTests
    {
        static SpiDevice _spiDevice;
        static SpiConnectionSettings _connectinSettings;

        [Setup]
        public void TryToSeeIfSpiIsSupported()
        {
            try
            {
                // Note: the ChipSelect pin should be adjusted to your device
                _connectinSettings = new SpiConnectionSettings(1, 12);
                _spiDevice = SpiDevice.Create(_connectinSettings);
            }
            catch (Exception)
            {
                Assert.SkipTest("Skipping tests as SPI bus or SPI settings are not correct.");
            }
        }

        [TestMethod]
        public void CheckSpiConectionSettings_00()
        {
            // Arrange
            SpiConnectionSettings connectionSettings = new SpiConnectionSettings(1, 12);
            connectionSettings.ChipSelectLineActiveState = PinValue.High;
            connectionSettings.ClockFrequency = 1_000_000;
            connectionSettings.DataBitLength = 8;
            connectionSettings.DataFlow = DataFlow.LsbFirst;
            connectionSettings.Mode = SpiMode.Mode2;
            connectionSettings.Configuration = SpiBusConfiguration.HalfDuplex;


            // Assert
            Assert.Equal(12, connectionSettings.ChipSelectLine);
            Assert.True(PinValue.High == connectionSettings.ChipSelectLineActiveState);
            Assert.Equal(1_000_000, connectionSettings.ClockFrequency);
            Assert.Equal(8, connectionSettings.DataBitLength);
            Assert.True(DataFlow.LsbFirst == connectionSettings.DataFlow);
            Assert.True(SpiMode.Mode2 == connectionSettings.Mode);
            Assert.Equal(1, connectionSettings.BusId);
            Assert.Equal((int)SpiBusConfiguration.HalfDuplex, (int)connectionSettings.Configuration);
        }

        [TestMethod]
        public void CheckSpiConectionSettings_01()
        {
            // Arrange
            SpiConnectionSettings connectionSettings = new SpiConnectionSettings(1);
            connectionSettings.ClockFrequency = 1_000_000;
            connectionSettings.DataBitLength = 16;

            // Assert
            Assert.Equal(-1, connectionSettings.ChipSelectLine);
            Assert.Equal(1_000_000, connectionSettings.ClockFrequency);
            Assert.Equal(8, connectionSettings.DataBitLength);
            Assert.True(DataFlow.MsbFirst == connectionSettings.DataFlow);
            Assert.True(SpiMode.Mode0 == connectionSettings.Mode);
            Assert.Equal(1, connectionSettings.BusId);
            Assert.Equal((int)SpiBusConfiguration.FullDuplex, (int)connectionSettings.Configuration);
        }


        [TestMethod]
        public void CheckSpiConectionSettingsClone()
        {
            SpiConnectionSettings connectionSettings = new SpiConnectionSettings(1, 12);
            connectionSettings.ChipSelectLineActiveState = PinValue.High;
            connectionSettings.ClockFrequency = 1_000_000;
            connectionSettings.DataBitLength = 8;
            connectionSettings.DataFlow = DataFlow.LsbFirst;
            connectionSettings.Mode = SpiMode.Mode2;
            connectionSettings.Configuration = SpiBusConfiguration.HalfDuplex;

            // clone SpiConnectionSettings
            var connectionSettingsClone = new SpiConnectionSettings(connectionSettings);

            // now compare all properties
            Assert.Equal(connectionSettingsClone.ChipSelectLine, connectionSettings.ChipSelectLine);
            Assert.True(connectionSettingsClone.ChipSelectLineActiveState == connectionSettings.ChipSelectLineActiveState);
            Assert.Equal(connectionSettingsClone.ClockFrequency, connectionSettings.ClockFrequency);
            Assert.Equal(connectionSettingsClone.DataBitLength, connectionSettings.DataBitLength);
            Assert.True(connectionSettingsClone.DataFlow == connectionSettings.DataFlow);
            Assert.True(connectionSettingsClone.Mode == connectionSettings.Mode);
            Assert.Equal(connectionSettingsClone.BusId, connectionSettings.BusId);
            Assert.Equal((int)connectionSettingsClone.Configuration, (int)connectionSettings.Configuration);
        }

        [TestMethod]
        public void CheckSpiConnectionImmutable()
        {
            // Arrange
            // Act
            var connToTest = _spiDevice.ConnectionSettings;
            connToTest.ChipSelectLine = 42;
            connToTest.DataBitLength = 16;

            //Assert
            Assert.NotEqual(_connectinSettings.DataBitLength, connToTest.DataBitLength);
            Assert.NotEqual(_connectinSettings.ChipSelectLine, connToTest.ChipSelectLine);
        }

        [TestMethod]
        public void FullTrasferSpanByte()
        {
            Debug.WriteLine("For this test, make sure you connect MOSI to MISO");
            // Arrange
            SpanByte writeBuffer = new byte[4] { 0xAA, 0xBB, 0xCC, 0x42 };
            SpanByte readBuffer = new byte[4];

            // Act
            _spiDevice.TransferFullDuplex(writeBuffer, readBuffer);

            // Assert
            Assert.Equal(writeBuffer.ToArray(), readBuffer.ToArray());
        }

        [TestMethod]
        public void FullTrasferPartialSpanByte()
        {
            Debug.WriteLine("For this test, make sure you connect MOSI to MISO");
            // Arrange
            SpanByte writeBuffer = new byte[4] { 0xAA, 0xBB, 0xCC, 0x42 };
            SpanByte toWriteAndAct = writeBuffer.Slice(1, 2);
            Debug.WriteLine($"Buffer to send length: {toWriteAndAct.Length}");
            SpanByte readBuffer = new byte[toWriteAndAct.Length];

            // Act
            _spiDevice.TransferFullDuplex(toWriteAndAct, readBuffer);

            // Assert
            Assert.Equal(toWriteAndAct.ToArray(), readBuffer.ToArray());
        }

        [TestMethod]
        public void FullTransferUshort()
        {
            Debug.WriteLine("For this test, make sure you connect MOSI to MISO");
            // Arrange
            ushort[] writeBuffer = new ushort[4] { 0xAABC, 0x00BB, 0xCC00, 0x4242 };
            ushort[] readBuffer = new ushort[4];

            // Act
            _spiDevice.TransferFullDuplex(writeBuffer, readBuffer);

            // Assert
            Assert.Equal(writeBuffer, readBuffer);
        }

        [TestMethod]
        public void SpiBusInfo_Tests()
        {
            // Arrange
            SpiBusInfo spiBusInfo = SpiDevice.GetBusInfo(1);

            Debug.WriteLine($"{nameof(spiBusInfo.MaxClockFrequency)}: {spiBusInfo.MaxClockFrequency}");
            Debug.WriteLine($"{nameof(spiBusInfo.MinClockFrequency)}: {spiBusInfo.MinClockFrequency}");
        }

        [TestMethod]
        public void TryToOpenSecondTimeSpi()
        {
            Assert.Throws(typeof(SpiDeviceAlreadyInUseException), () =>
            {
                SpiDevice newSpi = SpiDevice.Create(new SpiConnectionSettings(1, 12));
            });
        }

        [Cleanup]
        public void DisposeSpiDevice()
        {
            _spiDevice?.Dispose();
        }
    }
}
