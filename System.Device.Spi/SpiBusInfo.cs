//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Runtime.CompilerServices;

namespace System.Device.Spi
{
    /// <summary>
    /// Represents the info about a SPI bus.
    /// </summary>
    public sealed class SpiBusInfo
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _controllerId;

        internal SpiBusInfo(int spiBus)
        {
            _controllerId = spiBus;

        }

        /// <summary>
        /// Gets the number of chip select lines available on the bus.
        /// </summary>
        /// <value>
        /// Number of chip select lines.
        /// </value>
        public int ChipSelectLineCount
        {
            get { return NativeChipSelectLineCount(); }
        }

        /// <summary>
        /// Maximum clock cycle frequency of the bus.
        /// </summary>
        /// <value>
        /// The clock cycle in Hz.
        /// </value>
        public int MaxClockFrequency
        {
            get { return NativeMaxClockFrequency(); }
        }

        /// <summary>
        /// Minimum clock cycle frequency of the bus.
        /// </summary>
        /// <value>
        /// The clock cycle in Hz.
        /// </value>
        public int MinClockFrequency
        {
            get { return NativeMinClockFrequency(); }
        }

        /// <summary>
        /// Gets the bit lengths that can be used on the bus for transmitting data.
        /// </summary>
        /// <value>
        /// The supported data lengths.
        /// </value>
        public int[] SupportedDataBitLengths
        {
            get
            {
                return new[] { 8, 16 };
            }
        }

        #region Native Calls


        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeChipSelectLineCount();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeMaxClockFrequency();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeMinClockFrequency();


        #endregion




    }
}
