﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScooterController
{
    class InstructionInterpreter
    {

        private readonly List<string> instructionRawLines;

        private int counterRawLine = 0;

        public InstructionInterpreter(string filename = "instruction.txt")
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
                var line = CleanUpRawLine(rawLine);
                if (line != null)
                {
                    var pair = line.Split(HardwareInstruction.OperatorSeparators, StringSplitOptions.RemoveEmptyEntries);
                    Console.WriteLine("~" + pair.Count() + "~" + pair[0]);
                }
            }

            return new HardwareInstruction();
        }

        private string GetNextRawLine()
        {
            return this.counterRawLine >= instructionRawLines.Count 
                ? null 
                : instructionRawLines[this.counterRawLine++];
        }

        private static string CleanUpRawLine(string rawLine)
        {
            var commentIndex = rawLine.IndexOf(HardwareInstruction.CommentSymbol, StringComparison.InvariantCultureIgnoreCase);
            var line = commentIndex >= 0 ? rawLine.Substring(0, commentIndex) : rawLine;
            line = line.Trim();
            return !string.IsNullOrWhiteSpace(line) ? line : null;
        }
    }
}
