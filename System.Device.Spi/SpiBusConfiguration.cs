//
// Copyright (c) .NET Foundation and Contributors.
// See LICENSE file in the project root for full license information.
//

namespace System.Device.Spi
{
    /// <summary>
    /// Defines the bus configuration between the master and the slave device.
    /// </summary>
    public enum SpiBusConfiguration
    {
        /// <summary>
        /// Devices are connected in full duplex configuration, using 4-wires.
        /// All SPI signals are connected.
        /// </summary>
        FullDuplex,

        /// <summary>
        /// Devices are connected in half duplex configuration, using 3-wires.
        /// Only CS, SCK and MOSI (master) signals are used.
        /// </summary>
        HalfDuplex,

        /// <summary>
        /// Devices are connected in simplex configuration, using 2-wires.
        /// Only CS, SCK and MOSI (master) signals are used. Communication flow is from master to slave only.
        /// </summary>
        Simplex
    }
}
