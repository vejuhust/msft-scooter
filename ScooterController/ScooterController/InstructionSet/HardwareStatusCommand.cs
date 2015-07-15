
namespace ScooterController.InstructionSet
{
    class HardwareStatusCommand : HardwareSetBase
    {
        public double TimeOut { get; private set; }

        public bool HasTimeOut { get; private set; }

        public HardwareStatusCommand(HardwareOperator Operator)
        {
            this.Operator = Operator;
            this.HasTimeOut = false;
        }

        public HardwareStatusCommand(HardwareOperator Operator, double TimeOut)
        {
            this.Operator = Operator;
            this.TimeOut = TimeOut;
            this.HasTimeOut = true;
        }
    }
}
