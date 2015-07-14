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
                else
                {
                    controller.ExecuteInstruction(instruction);
                }
            }
        }

        private static void ExecuteInstructionFromConsole()
        {
            Console.WriteLine("Welcome to Scooter Console!");
            var instructionCounter = 0;

            var parser = new InstructionInterpreter();
            var controller = new HardwareController();

            while (true)
            {
                instructionCounter++;

                Console.Write("\n>>{0:D4}>>> ", instructionCounter);
                var inputLine = Console.ReadLine();

                var instruction = parser.InterpretRawLine(inputLine, true);
                if (instruction == null)
                {
                    Console.WriteLine("[Invalid Instruction]");
                }
                else
                {
                    Console.WriteLine("[{0}{1}]", instruction.Operator, instruction.HasOperand ? " " + instruction.Operand : string.Empty);

                    if (instruction.Operator == HardwareOperator.NoOp || instruction.Operator == HardwareOperator.Exit)
                    {
                        break;
                    }
                    else
                    {
                        controller.ExecuteInstruction(instruction);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                ExecuteInstructionFromFile(args[0]);
            }
            else
            {
                ExecuteInstructionFromConsole();
            }
        }
    }
}
