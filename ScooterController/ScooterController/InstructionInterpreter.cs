﻿using System.Collections.Generic;
using System.IO;

namespace ScooterController
{
    class InstructionInterpreter
    {
        private readonly List<string> instructionRawLines;

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
            return new HardwareInstruction();
        }
    }
}