using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterController
{
    class HardwareController
    {
        public HardwareController()
        {
            if (UsbRelayDevice.Init() != 0)
            {
                Console.WriteLine("Couldn't initialize!");
                return;
            }

            string serial = "FDP2R";
            int deviceHandle = UsbRelayDevice.OpenWithSerialNumber(serial, serial.Length);
            int openResult = UsbRelayDevice.OpenOneRelayChannel(deviceHandle, 1);
            if (openResult == 1)
            {
                Console.WriteLine("Got error from OpenOneRelayChannel!");
                return;
            }
            else if (openResult == 2)
            {
                Console.WriteLine("Index is out of range on the usb relay device");
                return;
            }
            int closeResult = UsbRelayDevice.CloseOneRelayChannel(deviceHandle, 1);

            var x = UsbRelayDevice.Enumerate();
            if (x == null)
            {

            }
        }
    }
}
