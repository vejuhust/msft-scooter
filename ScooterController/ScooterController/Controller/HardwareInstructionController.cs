using ScooterController.Configuration;
using ScooterController.InstructionSet;

namespace ScooterController.Controller
{
    public class HardwareInstructionController : HardwareBaseController
    {
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
                case HardwareOperator.SetSpeed:
                    this.SetSpeed(instruction.Operand);
                    break;
            }
        }

        private void ActMoveForward(long unit)
        {
            LogInfo(string.Format("Move Forward for {0} Units", unit));

            var channel = HardwareSetting.ChannelMoveForward;
            var intervalSeconds = HardwareSetting.IntervalMoveForward * unit;

            this.OpenChannel(channel);
            this.Suspend(intervalSeconds);
            this.CloseChannel(channel);
        }

        private void ActMoveBack(long unit)
        {
            LogInfo(string.Format("Move Back for {0} Units", unit));

            var channel = HardwareSetting.ChannelMoveBack;
            var intervalSeconds = HardwareSetting.IntervalMoveBack * unit;

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

        private void SetSpeed(long targetSpeed)
        {
            if (!(1 <= targetSpeed && targetSpeed <= 3))
            {
                LogError(string.Format("Target Speed '{0}' is invalid", targetSpeed));
            }

            long speedSetTimes = 0;
            if (this.currentSpeed == targetSpeed)
            {
                LogInfo(string.Format("Speed '{0}' Remains Unchanged", this.currentSpeed));
            }
            else if (this.currentSpeed < targetSpeed)
            {
                LogInfo(string.Format("Speed Up From {0} To {1}", this.currentSpeed, targetSpeed));
                speedSetTimes = targetSpeed - this.currentSpeed;
            }
            else
            {
                LogInfo(string.Format("Slow Down From {0} To {1}", this.currentSpeed, targetSpeed));
                speedSetTimes = 3 + targetSpeed - this.currentSpeed;
            }

            var channel = HardwareSetting.ChannelSpeedUp;
            var intervalSeconds = HardwareSetting.IntervalSpeed;
            for (var i = 0; i < (int)speedSetTimes; i++)
            {
                this.OpenChannel(channel);
                this.Suspend(intervalSeconds);
                this.CloseChannel(channel);

                this.Suspend(HardwareSetting.IntervalInstruction);
            }

            this.currentSpeed = targetSpeed;
        }
    }
}
