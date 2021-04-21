using System;
using System.Device.Spi;
using System.Diagnostics;
using System.Threading;

namespace TestApp
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from SPI");

            SpiDevice spiDevice = new SpiDevice(new SpiConnectionSettings(1, 18));
            SpanByte writeBuffer = new byte[2] { 0b1010_1010, 0b0101_0101 };
            SpanByte readBuffer = new byte[2];
            ushort[] output = new ushort[2] { 42, 84 };
            ushort[] input = new ushort[2];
            for (int i = 0; i < int.MaxValue; i++)
            {
                //spiDevice.Write(writeBuffer);
                spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
                Debug.WriteLine($"{BitConverter.ToString(readBuffer.ToArray())}");
                spiDevice.TransferFullDuplex(output, input);
                for (int j = 0; j < input.Length; j++)
                    Debug.Write($"{input[j]}-");
                Debug.WriteLine("");
                Thread.Sleep(100);
            }
            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
