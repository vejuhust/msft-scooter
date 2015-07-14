
using System;

namespace ScooterController
{
    class HardwareController
    {
        private readonly int deviceHandle;

        public static void LogError(string message)
        {
            throw new Exception(message);
        }

        public HardwareController(string serialNumber = "FDP2R")
        {
            if (UsbRelayDevice.Init() != 0)
            {
                LogError("Couldn't initialize!");
            }

            this.deviceHandle = UsbRelayDevice.OpenWithSerialNumber(serialNumber, serialNumber.Length);
        }
        ~HardwareController()
        {
            UsbRelayDevice.Close(this.deviceHandle);
        }
    }
}
