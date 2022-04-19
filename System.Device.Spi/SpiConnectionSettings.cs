//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Device.Gpio;

namespace System.Device.Spi
{
    /// <summary>
    /// The connection settings of a device on a SPI bus.
    /// </summary>
    public sealed class SpiConnectionSettings
    {
        private int _csLine;

        // 500 KHz
        private int _clockFrequency = 500_000;

        // 1 byte
        private int _databitLength = 8;  
        private SpiMode _spiMode = SpiMode.Mode0;
        private SpiSharingMode _spiSharingMode;
        private DataFlow _dataFlow = DataFlow.MsbFirst;
        private int _busId;
        // Default is active low
        private int _chipSelectLineActiveState = 0;
        // default is full duplex (4-wires)
        private SpiBusConfiguration _busConfiguration = SpiBusConfiguration.FullDuplex;

        /// <summary>
        /// Initializes new instance of SpiConnectionSettings.
        /// </summary>

        private SpiConnectionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpiConnectionSettings"/> class.
        /// </summary>
        /// <param name="busId">The bus ID the device is connected to.</param>
        /// <param name="chipSelectLine">The chip select line used on the bus. In .NET nanoFramework, you need to have a
        /// valid GPIO Chip Select even if you don't use it.</param>
        public SpiConnectionSettings(int busId, int chipSelectLine)
        {
            BusId = busId;
            ChipSelectLine = chipSelectLine;
        }

        internal SpiConnectionSettings(SpiConnectionSettings other)
        {
            BusId = other.BusId;
            ChipSelectLine = other.ChipSelectLine;
            Mode = other.Mode;
            DataBitLength = other.DataBitLength;
            ClockFrequency = other.ClockFrequency;
            DataFlow = other.DataFlow;
            ChipSelectLineActiveState = other.ChipSelectLineActiveState;

            SharingMode = other.SharingMode;
        }

        /// <summary>
        /// The bus ID the device is connected to.
        /// </summary>
        public int BusId
        {
            get => _busId;

            set
            {
                _busId = value;
            }
        }

        /// <summary>
        /// The chip select line used on the bus.
        /// </summary>
        public int ChipSelectLine
        {
            get => _csLine;

            set
            {
                _csLine = value;
            }
        }

        /// <summary>
        /// The SPI mode being used.
        /// </summary>
        public SpiMode Mode
        {
            get => _spiMode;

            set
            {
                _spiMode = value;
            }
        }

        /// <summary>
        /// The length of the data to be transfered.
        /// </summary>
        public int DataBitLength
        {
            get => _databitLength;

            set
            {
                _databitLength = value;
            }
        }

        /// <summary>
        /// The frequency in which the data will be transferred.
        /// </summary>
        public int ClockFrequency
        {
            get => _clockFrequency;

            set
            {
                _clockFrequency = value;
            }
        }

        /// <summary>
        /// Specifies order in which bits are transferred first on the SPI bus.
        /// </summary>
        public DataFlow DataFlow
        {
            get => _dataFlow;

            set
            {
                _dataFlow = value;
            }
        }

        /// <summary>
        /// Specifies which value on chip select pin means "active".
        /// </summary>
        public PinValue ChipSelectLineActiveState
        {
            get => _chipSelectLineActiveState;
            set => _chipSelectLineActiveState = value == PinValue.Low ? 0 : 1;
        }

        /// <summary>
        /// Gets or sets the sharing mode for the SPI connection.
        /// </summary>
        public SpiSharingMode SharingMode
        {
            get => _spiSharingMode;

            set
            {
                _spiSharingMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the SPI bus configuration.
        /// </summary>
        /// <remarks>
        /// Default value is <see cref="SpiBusConfiguration.FullDuplex"/>.
        /// </remarks>
        public SpiBusConfiguration Configuration
        {
            get => _busConfiguration;

            set
            {
                _busConfiguration = value;
            }
        }
    }
}
