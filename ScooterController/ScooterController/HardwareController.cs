
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
            this.OpenChannel(HardwareSetting.ChannelPower);

            this.Connect();
        }

        ~HardwareController()
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

        public void ExecuteInstruction(HardwareInstruction instruction)
        {
            switch (instruction.Operator)
            {
                case HardwareOperator.MoveForward:
                    this.ActMoveForward(instruction.Operand);
                    break;
                case HardwareOperator.MoveBack:
                    this.ActMoveBack(instruction.Operand);
                    break;
                case HardwareOperator.TurnLeft:
                    this.ActTurnLeft(instruction.Operand);
                    break;
                case HardwareOperator.TurnRight:
                    this.ActTurnRight(instruction.Operand);
                    break;
            }
        }

        private void ActMoveForward(long unit)
        {
            LogInfo(string.Format("Move Forward for {0} Units", unit));

            var channel = HardwareSetting.ChannelMoveForward;
            var intervalSeconds = HardwareSetting.IntervalMove * unit;

            this.OpenChannel(channel);
            this.Suspend(intervalSeconds);
            this.CloseChannel(channel);
        }

        private void ActMoveBack(long unit)
        {
            LogInfo(string.Format("Move Back for {0} Units", unit));

            var channel = HardwareSetting.ChannelMoveBack;
            var intervalSeconds = HardwareSetting.IntervalMove * unit;

            this.OpenChannel(channel);
            this.Suspend(intervalSeconds);
            this.CloseChannel(channel);
        }

        private void ActTurnLeft(long unit)
        {
            LogInfo(string.Format("Turn Left for {0} Units", unit));

            var channel = HardwareSetting.ChannelTurnLeft;
            var intervalSeconds = HardwareSetting.IntervalTurn * unit;

            this.OpenChannel(channel);
            this.Suspend(intervalSeconds);
            this.CloseChannel(channel);

            this.Suspend(HardwareSetting.IntervalInstruction);
        }
        private void ActTurnRight(long unit)
        {
            LogInfo(string.Format("Turn Right for {0} Units", unit));

            var channel = HardwareSetting.ChannelTurnRight;
            var intervalSeconds = HardwareSetting.IntervalTurn * unit;

            this.OpenChannel(channel);
            this.Suspend(intervalSeconds);
            this.CloseChannel(channel);

            this.Suspend(HardwareSetting.IntervalInstruction);
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
