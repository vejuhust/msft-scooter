using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterController
{
    class InstructionInterrupter
    {
        private readonly List<string> instructionRawLines;

        public InstructionInterrupter(string filename = "instruction.txt")
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
