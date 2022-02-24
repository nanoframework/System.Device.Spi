//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Runtime.CompilerServices;

namespace System.Device.Spi
{
    /// <summary>
    /// Base class for SPI bus information.
    /// </summary>
    public sealed class SpiBusInfo
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly int _controllerId;

        internal SpiBusInfo(int spiBus)
        {
            _controllerId = spiBus;
        }

        /// <summary>
        /// Maximum clock cycle frequency of the bus.
        /// </summary>
        /// <value>
        /// The clock cycle in Hz.
        /// </value>
        public int MaxClockFrequency => NativeMaxClockFrequency();

        /// <summary>
        /// Minimum clock cycle frequency of the bus.
        /// </summary>
        /// <value>
        /// The clock cycle in Hz.
        /// </value>
        public int MinClockFrequency => NativeMinClockFrequency();

        #region Native Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeMaxClockFrequency();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeMinClockFrequency();

        #endregion
    }
}
