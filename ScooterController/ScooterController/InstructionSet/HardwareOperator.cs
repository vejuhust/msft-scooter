
namespace ScooterController.InstructionSet
{
    enum HardwareOperator
    {
        NoOp,           /* 0: <blank> */
        PowerOn,        /* 1: on */
        PowerOff,       /* 2: off */
        SetSpeed,       /* 3: sp */
        MoveForward,    /* 4: fd */
        MoveBack,       /* 5: bk */
        TurnLeft,       /* 6: lt */
        TurnRight,      /* 7: rt */
        Brake,          /* 8: ?? */
        Exit,           /* 9: exit */
    }
}
