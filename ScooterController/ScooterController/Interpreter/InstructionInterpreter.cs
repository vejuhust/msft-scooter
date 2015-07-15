using ScooterController.InstructionSet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScooterController.Interpreter
{
    class InstructionInterpreter
    {
        private readonly List<string> instructionRawLines;

        private int counterRawLine = 0;

        private static Dictionary<HardwareOperator, string> NullaryOperatorMapping = new Dictionary<HardwareOperator, string>()
        {
            { HardwareOperator.PowerOn, "on" },     
            { HardwareOperator.PowerOff, "off" },     
            { HardwareOperator.Exit, "exit" },     
        };

        private static Dictionary<HardwareOperator, string> UnaryOperatorMapping = new Dictionary<HardwareOperator, string>()
        {
            { HardwareOperator.SetSpeed, "sp" },     
            { HardwareOperator.MoveForward, "fd" },     
            { HardwareOperator.MoveBack, "bk" },     
            { HardwareOperator.TurnLeft, "lt" },     
            { HardwareOperator.TurnRight, "rt" },     
        };

        public InstructionInterpreter()
        {
            this.instructionRawLines = null;
        }

        public InstructionInterpreter(string filename)
        {
            using (var file = new StreamReader(filename))
            {
                string line;

                var lines = new List<string>();
                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                this.instructionRawLines = lines;
            }
        }

        public HardwareInstruction GetNextInstruction()
        {
            string rawLine;
            while ((rawLine = this.GetNextRawLine()) != null)
            {
                HardwareInstruction instructionResult;
                if ((instructionResult = this.InterpretRawLine(rawLine)) != null)
                {
                    return instructionResult;
                }
            }

            return new HardwareInstruction();
        }

        private string GetNextRawLine()
        {
            return this.instructionRawLines != null && this.counterRawLine < this.instructionRawLines.Count
                ? this.instructionRawLines[this.counterRawLine++]
                : null;
        }

        private static string CleanUpRawLine(string rawLine)
        {
            var commentIndex = rawLine.IndexOf(HardwareInstruction.CommentSymbol, StringComparison.InvariantCultureIgnoreCase);
            var line = commentIndex >= 0 ? rawLine.Substring(0, commentIndex) : rawLine;
            line = line.Trim().ToLowerInvariant();
            return !string.IsNullOrWhiteSpace(line) ? line : null;
        }

        public HardwareInstruction InterpretRawLine(string rawLine)
        {
            var line = CleanUpRawLine(rawLine);
            if (line == null)
            {
                return null;
            }

            var result = InterpretInstructionLine(line);
            if (result == null)
            {
                throw new Exception("Unknown Instruction");
            }
            else
            {
                return result;
            }
        }

        private HardwareInstruction InterpretInstructionLine(string line)
        {
            var pair = line.Split(HardwareInstruction.OperatorSeparators, StringSplitOptions.RemoveEmptyEntries);

            switch (pair.Count())
            {
                case 1:
                    {
                        var rawOperator = pair[0];
                        var instructionOperator = StringToHardwareOperator(NullaryOperatorMapping, rawOperator);
                        if (instructionOperator != HardwareOperator.NoOp)
                        {
                            return new HardwareInstruction(instructionOperator);
                        }
                    }
                    break;
                case 2:
                    {
                        var rawOperator = pair[0];
                        var instructionOperator = StringToHardwareOperator(UnaryOperatorMapping, rawOperator);
                        if (instructionOperator != HardwareOperator.NoOp)
                        {
                            var rawOperand = pair[1];
                            var instructionOperand = StringToHardwareOperand(rawOperand);
                            return new HardwareInstruction(instructionOperator, instructionOperand);
                        }
                    }
                    break;
            }

            return null;
        }

        private static HardwareOperator StringToHardwareOperator(Dictionary<HardwareOperator, string> mapping, string rawString)
        {
            return (from op in mapping where op.Value == rawString select op.Key).FirstOrDefault();
        }

        private static long StringToHardwareOperand(string rawString)
        {
            return Convert.ToInt64(rawString);
        }
    }
}
