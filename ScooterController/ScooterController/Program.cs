using System;

namespace ScooterController
{
    class Program
    {
        private static void ExecuteInstructionFromFile(string filename)
        {
            var parser = new InstructionInterpreter(filename);
            var controller = new HardwareController();

            HardwareInstruction instruction;
            while ((instruction = parser.GetNextInstruction()) != null)
            {
                Console.WriteLine("\n[{0}{1}]", instruction.Operator, instruction.HasOperand ? " " + instruction.Operand : string.Empty);

                if (instruction.Operator == HardwareOperator.NoOp || instruction.Operator == HardwareOperator.Exit)
                {
                    break;
                }

                controller.ExecuteInstruction(instruction);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                ExecuteInstructionFromFile(args[0]);
            }
        }
    }
}
