using ScooterController.Configuration;
using ScooterController.HardwareAbstractionLayer;
using System;
using System.Threading;

namespace ScooterController.Controller
{
    class HardwareBaseController
    {
        private readonly int deviceHandle;

        internal long currentSpeed = 1;

        public static void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogError(string message)
        {
            throw new Exception(message);
        }

        public HardwareBaseController()
        {
            if (UsbRelayDevice.Init() != 0)
            {
                LogError("Couldn't initialize!");
            }

            this.deviceHandle = UsbRelayDevice.OpenWithSerialNumber(HardwareSetting.SerialNumber, HardwareSetting.SerialNumber.Length);
            this.OpenChannel(HardwareSetting.ChannelPower);

            this.Connect();
        }

        ~HardwareBaseController()
        {
            this.CloseChannel(HardwareSetting.ChannelPower);
            UsbRelayDevice.Close(this.deviceHandle);
        }

        public void Connect()
        {
            LogInfo("Connecting");
            
            this.OpenChannel(HardwareSetting.ChannelConnect);
            this.Suspend(HardwareSetting.IntervalConnect);
            this.CloseChannel(HardwareSetting.ChannelConnect);
           
            // Interval between instructions
            this.Suspend(HardwareSetting.IntervalInstruction);
        }

        internal void Suspend(double timeoutSeconds)
        {
            var timeoutMilliseconds = Convert.ToInt32(1000 * timeoutSeconds);
            Thread.Sleep(timeoutMilliseconds);
        }

        internal void OpenChannel(int channelNumber)
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

        internal void CloseChannel(int channelNumber)
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
