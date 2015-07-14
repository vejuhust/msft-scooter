using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterController
{
    enum HardwareInstruction
    {
        NoOp,           /* <blank> */
        PowerOn,        /* on */
        PowerOff,       /* off */
        MoveForward,    /* fd */
        MoveBack,       /* bk */
        Brake,          /* ?? */
        SetSpeed,       /* sp */
        TurnLeft,       /* lt */
        TurnRight,      /* rt */
    }
}
