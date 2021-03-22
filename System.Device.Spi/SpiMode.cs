//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Device.Spi
{
    /// <summary>
    /// Defines how data is synchronized between devices on a SPI bus.
    /// Clock Polarity (CPOL) determines if clock signal is low or high when in idle state.
    /// Clock Phase (CPHA) determines when data is sampled relative to the clock signal.
    /// </summary>
    public enum SpiMode
    {
        /// <summary>
        /// CPOL 0, CPHA 0. Polarity is idled low and data is sampled on rising edge of the clock signal.
        /// </summary>
        Mode0,

        /// <summary>
        /// CPOL 0, CPHA 1. Polarity is idled low and data is sampled on falling edge of the clock signal.
        /// </summary>
        Mode1,

        /// <summary>
        /// CPOL 1, CPHA 0. Polarity is idled high and data is sampled on falling edge of the clock signal.
        /// </summary>
        Mode2,

        /// <summary>
        /// CPOL 1, CPHA 1. Polarity is idled high and data is sampled on rising edge of the clock signal.
        /// </summary>
        Mode3
    }
}
