using System;

namespace ScooterController
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = args.Length >= 1 
                ? new InstructionInterpreter(args[0])
                : new InstructionInterpreter();

            var controller = new HardwareController();

            HardwareInstruction instruction;
            while ((instruction = parser.GetNextInstruction()) != null)
            {
                Console.WriteLine("\n[{0}{1}]", instruction.Operator, instruction.HasOperand ? " " + instruction.Operand.ToString() : string.Empty);

                if (instruction.Operator == HardwareOperator.NoOp || instruction.Operator == HardwareOperator.Exit)
                {
                    break;
                }

                controller.ExecuteInstruction(instruction);
            }
        }
    }
}
