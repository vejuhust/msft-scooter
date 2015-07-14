using System;

namespace ScooterController
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new HardwareController();
            var parser = new InstructionInterpreter();

            HardwareInstruction instruction;
            while ((instruction = parser.GetNextInstruction()) != null)
            {
                if (instruction.Operator == HardwareOperator.NoOp || instruction.Operator == HardwareOperator.Exit)
                {
                    break;
                }

                Console.WriteLine("\n[{0} {1}]", instruction.Operator, instruction.HasOperand ? instruction.Operand.ToString() : string.Empty);
                controller.ExecuteInstruction(instruction);
            }
        }
    }
}
