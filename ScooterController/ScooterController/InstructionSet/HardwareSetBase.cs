namespace ScooterController.InstructionSet
{
    public class HardwareSetBase
    {
        public static char[] OperatorSeparators = { ' ', '\t' };

        public static string CommentSymbol = ";";

        public HardwareOperator Operator { get; internal set; }
    }
}
