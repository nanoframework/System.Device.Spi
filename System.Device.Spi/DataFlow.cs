//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Device.Spi
{
    /// <summary>
    /// Specifies order in which bits are transferred first on the SPI bus.
    /// </summary>
    public enum DataFlow
    {
        /// <summary>
        /// Most significant bit will be sent first (most of the devices use this value).
        /// </summary>
        MsbFirst,

        /// <summary>
        /// Least significant bit will be sent first.
        /// </summary>
        LsbFirst,
    }
}
