using System;
using System.Collections.Generic;
using System.IO;

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
            string line;
            while ((line = this.GetNextRawLine()) != null)
            {
                Console.WriteLine(line);
            }

            return new HardwareInstruction();
        }

        private string GetNextRawLine()
        {
            return this.counterRawLine >= instructionRawLines.Count 
                ? null 
                : instructionRawLines[this.counterRawLine++];
        }
    }
}
