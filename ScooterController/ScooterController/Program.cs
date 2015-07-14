﻿using System;

namespace ScooterController
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new HardwareController();
            controller.Connect();

            return;

            var parser = new InstructionInterpreter();

            HardwareInstruction instruction;
            while ((instruction = parser.GetNextInstruction()) != null)
            {
                Console.WriteLine("{0} {1}", instruction.Operator, instruction.HasOperand ? instruction.Operand.ToString() : string.Empty);

                if (instruction.Operator == HardwareOperator.NoOp)
                {
                    break;
                }
            }
        }
    }
}
