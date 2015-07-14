
namespace ScooterController
{
    class HardwareInstruction
    {
        public static char[] OperatorSeparators = {' ', '\t'};

        public static string CommentSymbol = ";";

        public HardwareOperator Operator { get; private set; }

        public long Operand { get; private set; }

        public bool HasOperand { get; private set; }

        public HardwareInstruction()
        {
            this.Operator = HardwareOperator.NoOp;
            this.HasOperand = false;
        }

        public HardwareInstruction(HardwareOperator Operator)
        {
            this.Operator = Operator;
            this.HasOperand = false;
        }

        public HardwareInstruction(HardwareOperator Operator, long Operand)
        {
            this.Operator = Operator;
            this.Operand = Operand;
            this.HasOperand = true;
        }
    }
}
