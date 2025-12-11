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
            SpiConnectionSettings connectionSettings = new(1, 12);
            connectionSettings.ChipSelectLineActiveState = PinValue.High;
            connectionSettings.ClockFrequency = 1_000_000;
            connectionSettings.DataBitLength = 8;
            connectionSettings.DataFlow = DataFlow.LsbFirst;
            connectionSettings.Mode = SpiMode.Mode2;
            connectionSettings.Configuration = SpiBusConfiguration.HalfDuplex;


            // Assert
            Assert.AreEqual(12, connectionSettings.ChipSelectLine);
            Assert.IsTrue(PinValue.High == connectionSettings.ChipSelectLineActiveState);
            Assert.AreEqual(1_000_000, connectionSettings.ClockFrequency);
            Assert.AreEqual(8, connectionSettings.DataBitLength);
            Assert.IsTrue(DataFlow.LsbFirst == connectionSettings.DataFlow);
            Assert.IsTrue(SpiMode.Mode2 == connectionSettings.Mode);
            Assert.AreEqual(1, connectionSettings.BusId);
            Assert.AreEqual((int)SpiBusConfiguration.HalfDuplex, (int)connectionSettings.Configuration);
        }

        [TestMethod]
        public void CheckSpiConectionSettings_01()
        {
            // Arrange
            SpiConnectionSettings connectionSettings = new(1);
            connectionSettings.ClockFrequency = 1_000_000;
            connectionSettings.DataBitLength = 16;

            // Assert
            Assert.AreEqual(-1, connectionSettings.ChipSelectLine);
            Assert.AreEqual(1_000_000, connectionSettings.ClockFrequency);
            Assert.AreEqual(8, connectionSettings.DataBitLength);
            Assert.IsTrue(DataFlow.MsbFirst == connectionSettings.DataFlow);
            Assert.IsTrue(SpiMode.Mode0 == connectionSettings.Mode);
            Assert.AreEqual(1, connectionSettings.BusId);
            Assert.AreEqual((int)SpiBusConfiguration.FullDuplex, (int)connectionSettings.Configuration);
        }


        [TestMethod]
        public void CheckSpiConectionSettingsClone()
        {
            SpiConnectionSettings connectionSettings = new(1, 12);
            connectionSettings.ChipSelectLineActiveState = PinValue.High;
            connectionSettings.ClockFrequency = 1_000_000;
            connectionSettings.DataBitLength = 8;
            connectionSettings.DataFlow = DataFlow.LsbFirst;
            connectionSettings.Mode = SpiMode.Mode2;
            connectionSettings.Configuration = SpiBusConfiguration.HalfDuplex;

            // clone SpiConnectionSettings
            var connectionSettingsClone = new SpiConnectionSettings(connectionSettings);

            // now compare all properties
            Assert.AreEqual(connectionSettingsClone.ChipSelectLine, connectionSettings.ChipSelectLine);
            Assert.IsTrue(connectionSettingsClone.ChipSelectLineActiveState == connectionSettings.ChipSelectLineActiveState);
            Assert.AreEqual(connectionSettingsClone.ClockFrequency, connectionSettings.ClockFrequency);
            Assert.AreEqual(connectionSettingsClone.DataBitLength, connectionSettings.DataBitLength);
            Assert.IsTrue(connectionSettingsClone.DataFlow == connectionSettings.DataFlow);
            Assert.IsTrue(connectionSettingsClone.Mode == connectionSettings.Mode);
            Assert.AreEqual(connectionSettingsClone.BusId, connectionSettings.BusId);
            Assert.AreEqual((int)connectionSettingsClone.Configuration, (int)connectionSettings.Configuration);
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
            Assert.AreNotEqual(_connectinSettings.DataBitLength, connToTest.DataBitLength);
            Assert.AreNotEqual(_connectinSettings.ChipSelectLine, connToTest.ChipSelectLine);
        }

        [TestMethod]
        public void FullTrasferSpanByte()
        {
            Debug.WriteLine("For this test, make sure you connect MOSI to MISO");
            // Arrange
            ReadOnlySpan<byte> writeBuffer = new byte[4] { 0xAA, 0xBB, 0xCC, 0x42 };
            Span<byte> readBuffer = new(new byte[4]);

            // Act
            _spiDevice.TransferFullDuplex(writeBuffer, readBuffer);

            // Assert
            Assert.AreEqual(writeBuffer.ToArray(), readBuffer.ToArray());
        }

        [TestMethod]
        public void FullTrasferPartialSpanByte()
        {
            Debug.WriteLine("For this test, make sure you connect MOSI to MISO");
            // Arrange
            Span<byte> writeBuffer = new(new byte[4] { 0xAA, 0xBB, 0xCC, 0x42 });
            
            Span<byte> toWriteAndAct = writeBuffer.Slice(1, 2);
            
            Debug.WriteLine($"Buffer to send length: {toWriteAndAct.Length}");
            
            Span<byte> readBuffer = new(new byte[toWriteAndAct.Length]);

            // Act
            _spiDevice.TransferFullDuplex(toWriteAndAct, readBuffer);

            // Assert
            Assert.AreEqual(toWriteAndAct.ToArray(), readBuffer.ToArray());
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
            Assert.AreEqual(writeBuffer, readBuffer);
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
            Assert.ThrowsException(typeof(SpiDeviceAlreadyInUseException), () =>
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
