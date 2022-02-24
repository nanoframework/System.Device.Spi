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
        public void CheckSpiConectionSettings()
        {
            // Arrange
            SpiConnectionSettings connectinSettings = new SpiConnectionSettings(1, 12);
            connectinSettings.ChipSelectLineActiveState = PinValue.High;
            connectinSettings.ClockFrequency = 1_000_000;
            connectinSettings.DataBitLength = 8;
            connectinSettings.DataFlow = DataFlow.LsbFirst;
            connectinSettings.Mode = SpiMode.Mode2;
            connectinSettings.SharingMode = SpiSharingMode.Exclusive;

            // Assert
            Assert.Equal(12, connectinSettings.ChipSelectLine);
            Assert.True(PinValue.High == connectinSettings.ChipSelectLineActiveState);
            Assert.Equal(1_000_000, connectinSettings.ClockFrequency);
            Assert.Equal(8, connectinSettings.DataBitLength);
            Assert.True(DataFlow.LsbFirst == connectinSettings.DataFlow);
            Assert.True(SpiMode.Mode2 == connectinSettings.Mode);
            Assert.True(SpiSharingMode.Exclusive == connectinSettings.SharingMode);
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
