﻿
namespace ScooterController.Configuration
{
    static class HardwareSetting
    {
        //public static string SerialNumber = "FDP2R";
        public static string SerialNumber = "FPIWS";


        public static int ChannelPower = 5;

        public static int ChannelConnect = 3;

        public static int ChannelSpeedUp = 4;

        public static int ChannelMoveForward = 1;

        public static int ChannelMoveBack = 2;
        
        public static int ChannelTurnLeft = 7;
        
        public static int ChannelTurnRight = 8;

        public static int ChannelBrake = 6;

        public static double IntervalConnect = 5;

        public static double IntervalInstruction = 0.5;

        public static double IntervalMoveForward = 1.0;

        public static double IntervalMoveBack = 1.9;

        public static double IntervalTurn = 0.15;

        public static double IntervalSpeed = 0.2;
    }
}
