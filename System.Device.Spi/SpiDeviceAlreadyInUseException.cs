//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace System.Device.Spi
{
    /// <summary>
    /// Exception thrown when a check in driver's constructor finds a device that already exists with the same settings (SPI bus AND chip select line)
    /// </summary>
    [Serializable]
    public class SpiDeviceAlreadyInUseException : Exception
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() { return base.Message; }
    }
}
