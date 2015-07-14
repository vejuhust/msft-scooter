using System;

namespace ScooterController
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new HardwareController();
            controller.ExecuteInstruction(new HardwareInstruction(HardwareOperator.MoveForward, 1));
            controller.ExecuteInstruction(new HardwareInstruction(HardwareOperator.MoveBack, 1));
            controller.ExecuteInstruction(new HardwareInstruction(HardwareOperator.TurnLeft, 1));
            controller.ExecuteInstruction(new HardwareInstruction(HardwareOperator.TurnRight, 1));
            controller.ExecuteInstruction(new HardwareInstruction(HardwareOperator.SetSpeed, 3));
                                                                                                
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
