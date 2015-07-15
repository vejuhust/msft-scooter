using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterController
{
    internal class HardwareSetBase
    {
        public static char[] OperatorSeparators = { ' ', '\t' };

        public static string CommentSymbol = ";";

        public HardwareOperator Operator { get; internal set; }
    }
}
