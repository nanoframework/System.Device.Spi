// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Device.Spi
{
    /// <summary>
    /// The communications channel to a device on a SPI bus.
    /// </summary>
    public class SpiDevice : IDisposable
    {
        private Windows.Devices.Spi.SpiDevice _device;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly Spi​Connection​Settings _connectionSettings;
        // For the ReadByte and WriteByte operations
        private byte[] bufferSingleOperation = new byte[1];

        /// <summary>
        /// The connection settings of a device on a SPI bus. The connection settings are immutable after the device is created
        /// so the object returned will be a clone of the settings object.
        /// </summary>
        public SpiConnectionSettings ConnectionSettings
        {
            get => new SpiConnectionSettings(_connectionSettings);
        }

        /// <summary>
        /// Reads a byte from the SPI device.
        /// </summary>
        /// <returns>A byte read from the SPI device.</returns>
        public byte ReadByte()
        {
            _device.Read(bufferSingleOperation);
            return bufferSingleOperation[0];
        }

        /// <summary>
        /// Reads data from the SPI device.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to read the data from the SPI device.
        /// The length of the buffer determines how much data to read from the SPI device.
        /// </param>
        public void Read(SpanByte buffer)
        {
            // This is allocating an intermediate buffer and then copy back the data to 
            // the SpanByte. This is intend to be changed in a native implementation
            byte[] toRead = new byte[buffer.Length];
            _device.Read(toRead);
            for (int i = 0; i < toRead.Length; i++)
            {
                buffer[i] = toRead[i];
            }
        }

        /// <summary>
        /// Writes a byte to the SPI device.
        /// </summary>
        /// <param name="value">The byte to be written to the SPI device.</param>
        public void WriteByte(byte value)
        {
            bufferSingleOperation[0] = value;
            _device.Write(bufferSingleOperation);
        }

        /// <summary>
        /// Writes data to the SPI device.
        /// </summary>
        /// <param name="buffer">
        /// The buffer that contains the data to be written to the SPI device.
        /// </param>
        public void Write(SpanByte buffer)
        {
            // This is allocating an intermediate buffer using the buffer of 
            // the SpanByte. This is intend to be changed in a native implementation
            _device.Write(buffer.ToArray());
        }

        /// <summary>
        /// Writes and reads data from the SPI device.
        /// </summary>
        /// <param name="writeBuffer">The buffer that contains the data to be written to the SPI device.</param>
        /// <param name="readBuffer">The buffer to read the data from the SPI device.</param>
        public void TransferFullDuplex(SpanByte writeBuffer, SpanByte readBuffer)
        {
            // This is allocating an intermediate buffer using the buffer of 
            // the SpanByte. This is intend to be changed in a native implementation
            byte[] toRead = new byte[readBuffer.Length];
            _device.TransferFullDuplex(writeBuffer.ToArray(), toRead);
            for (int i = 0; i < toRead.Length; i++)
            {
                readBuffer[i] = toRead[i];
            }
        }

        /// <summary>
        /// Creates a communications channel to a device on a SPI bus running on the current hardware
        /// </summary>
        /// <param name="settings">The connection settings of a device on a SPI bus.</param>
        /// <returns>A communications channel to a device on a SPI bus running on Windows 10 IoT.</returns>
        public static SpiDevice Create(SpiConnectionSettings settings)
        {
            return new SpiDevice(settings);
        }

        /// <summary>
        /// Creates a communications channel to a device on a SPI bus running on the current hardware
        /// </summary>
        /// <param name="settings">The connection settings of a device on a SPI bus.</param>
        public SpiDevice(SpiConnectionSettings settings)
        {
            _connectionSettings = settings;
            _device = Windows.Devices.Spi.SpiDevice.FromId($"SPI{settings.BusId}", new Windows.Devices.Spi.SpiConnectionSettings(settings.ChipSelectLine)
            {
                BitOrder = settings.DataFlow == DataFlow.MsbFirst ? Windows.Devices.Spi.DataBitOrder.MSB : Windows.Devices.Spi.DataBitOrder.LSB,
                ClockFrequency = settings.ClockFrequency,
                DataBitLength = settings.DataBitLength,
                Mode = (Windows.Devices.Spi.SpiMode)settings.Mode,
                SharingMode = (Windows.Devices.Spi.SpiSharingMode)settings.SharingMode
            });
        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if explicitly disposing, <see langword="false"/> if in finalizer</param>
        protected void Dispose(bool disposing)
        {
            // Nothing to do in base class.
        }
    }
}
