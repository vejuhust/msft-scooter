
using System;
using System.Threading;

namespace ScooterController
{
    class HardwareController
    {
        private readonly int deviceHandle;

        public static void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogError(string message)
        {
            throw new Exception(message);
        }

        public HardwareController()
        {
            if (UsbRelayDevice.Init() != 0)
            {
                LogError("Couldn't initialize!");
            }

            this.deviceHandle = UsbRelayDevice.OpenWithSerialNumber(HardwareSetting.SerialNumber, HardwareSetting.SerialNumber.Length);
            this.OpenChannel(5);
        }

        ~HardwareController()
        {
            this.CloseChannel(5);
            UsbRelayDevice.Close(this.deviceHandle);
        }

        public void Connect()
        {
            // Connect
            this.OpenChannel(3);
            this.Suspend(5);
            this.CloseChannel(3);
           
            // Interval between instructions
            this.Suspend(0.5);

            // Back
            this.OpenChannel(2);
            this.Suspend(1);
            this.CloseChannel(2);
        }

        private void Suspend(double timeoutSeconds)
        {
            var timeoutMilliseconds = Convert.ToInt32(1000 * timeoutSeconds);
            Thread.Sleep(timeoutMilliseconds);
        }

        private void OpenChannel(int channelNumber)
        {
            var openResult = UsbRelayDevice.OpenOneRelayChannel(this.deviceHandle, channelNumber);
            switch (openResult)
            {
                case 1:
                    LogError(string.Format("Got error from OpenOneRelayChannel({0}, {1})!", this.deviceHandle, channelNumber));
                    break;
                case 2:
                    LogError(string.Format("Index '{0}' is out of range on the USB relay device in OpenOneRelayChannel", channelNumber));
                    break;
            }
        }

        private void CloseChannel(int channelNumber)
        {
            var closeResult = UsbRelayDevice.CloseOneRelayChannel(this.deviceHandle, channelNumber);
            switch (closeResult)
            {
                case 1:
                    LogError(string.Format("Got error from CloseOneRelayChannel({0}, {1})!", this.deviceHandle, channelNumber));
                    break;
                case 2:
                    LogError(string.Format("Index '{0}' is out of range on the USB relay device in CloseOneRelayChannel", channelNumber));
                    break;
            }
        }
    }
}
