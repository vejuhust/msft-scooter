using ScooterController.Controller;
using ScooterController.HardwareAbstractionLayer;
using ScooterController.InstructionSet;
using ScooterController.Interpreter;
using System;
using System.Threading;

namespace ScooterController
{
    class Program
    {
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

        private static void ExecuteInstructionFromFile(string filename)
        {
            var parser = new InstructionInterpreter(filename);
            var controller = new HardwareInstructionController();

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
            var controller = new HardwareInstructionController();

            while (true)
            {
                instructionCounter++;

                Console.Write("\n>>{0:D4}>>> ", instructionCounter);
                var inputLine = Console.ReadLine();

                try
                {
                    var instruction = parser.InterpretRawLine(inputLine);
                    if (instruction == null)
                    {
                        Console.WriteLine("[Blank Instruction]");
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
                catch (Exception e)
                {
                    Console.WriteLine("[Invalid Instruction: {0}]", e.Message);
                }
            }
        }
    }
}
