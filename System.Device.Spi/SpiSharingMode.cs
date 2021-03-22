//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Device.Spi
{
    /// <summary>
    /// Defines the sharing mode for the SPI bus.
    /// </summary>
    public enum SpiSharingMode
    {
        /// <summary>
        /// SPI bus segment is not shared.
        /// </summary>
        Exclusive,

        /// <summary>
        /// SPI bus is shared.
        /// </summary>
        Shared
    }
}
