using System.IO;
using System.Security.Cryptography.X509Certificates;
using NavigatorManaged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ScooterController.Controller;
using ScooterController.InstructionSet;
using ScooterController.Interpreter;
namespace SmartGuide
{
    class Program
    {
        public class Coordinate
        {
            public Coordinate(double lat, double lgt)
            {
                this.lat = lat;
                this.lgt = lgt;
            }

            public double lat { get; set; }
            public double lgt { get; set; }
        }


        private const float margin = 0.001f;
        private const float epsilon = 0.0001f;
        public static float Position2Direcion(Coordinate srcCoordinates, Coordinate desCoordinates, Coordinate curCoordinates)
        {
            var y1 = srcCoordinates.lat;
            var y2 = desCoordinates.lat;
            var x1 = srcCoordinates.lgt;
            var x2 = desCoordinates.lgt;
            if (Math.Abs(x1 - x2) > epsilon)
            {
                double k = (y1 - y2) / (x1 - x2);
                var x = curCoordinates.lgt;
                var y = curCoordinates.lat;

                if ((y > k * x + margin) ^ (x2 < x1 - margin))
                {
                    return 1.0f; //turn right
                }
                else if ((y < k * x + margin) ^ (x2 > x1 + margin))
                {
                    return -1.0f; //turn left
                }
                else
                {
                    return 0.0f; // keep going
                }
            }
            else if (Math.Abs(y1 - y2) > epsilon)
            {
                var x = curCoordinates.lgt;
                var y = curCoordinates.lat;

                if ((x < 0) ^ (y2 < y1 - margin))
                {
                    return 1.0f; //turn right
                }
                else if ((x > 0) ^ (y2 > y1 + margin))
                {
                    return -1.0f; //turn left
                }
                else
                {
                    return 0.0f; // keep going
                }
            }
            else
            {
                return 100.0f; // reach destination
            }
            //stop

            //left

            //right
            return 0.0f;
        }
        private const long numIntervalTurn = 1;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var navigation = new Navigator(/*ChangePortName is needed*/);
            var logger = new Logger(navigation);
            //var route1 = navigation.GetRoute(@"北京市鼓楼东大街46号", @"三里屯");
            //var routing = new Routing();
            var destination = @"独墅湖体育馆";
            Double desLat = 31.265, desLgt = 120.7267;
            if (args.Length == 1)
            {
                var pos = args[0].Split(',');
                Double.TryParse(pos[0], out desLat);
                Double.TryParse(pos[1], out desLgt);
            }else if (args.Length == 2)
            {
                Double.TryParse(args[0], out desLat);
                Double.TryParse(args[1], out desLgt);
            }

            try
            {
                ////var srcLat = desLat + 0.01;//navigation.getLatitude();
                ////var srcLgt = desLgt + 0.01;//navigation.getLongitude();
                navigation.Connect();
                var s = new Stopwatch();
                s.Start();
                while (s.Elapsed.Seconds < 5)
                {
                    continue;
                }
                var currentLocation = navigation.GetCurrentLocation();
                Console.WriteLine(@"Current Location is :", navigation.GetCurrentLocation());
                var srcLat = navigation.getLatitude();
                var srcLgt = navigation.getLongitude();
                var srcCoordinate = new Coordinate(srcLat, srcLgt);
                var desCoordinate = new Coordinate(desLat, desLgt);
                ////var controller = new HardwareInstructionController();
                var op = HardwareOperator.Brake;
                var instruction = new HardwareInstruction(op, 2 * numIntervalTurn);
                ////controller.ExecuteInstruction(instruction);
                op = HardwareOperator.MoveForward;
                instruction = new HardwareInstruction(op, 2 * numIntervalTurn);
                ////controller.ExecuteInstruction(instruction);
                while (s.Elapsed < TimeSpan.FromSeconds(600) )
                {
                    if (s.Elapsed.Seconds%10 == 5)
                    {
                        var lat = navigation.getLatitude();
                        var lgt = navigation.getLongitude();
                        var currentCoordinate = new Coordinate(lat, lgt);
                        var direction = Position2Direcion(srcCoordinate, desCoordinate, currentCoordinate);
                        Console.WriteLine(direction);
                        op = HardwareOperator.NoOp;
                        if (direction > 99)
                        {
                            op = HardwareOperator.Brake;
                            Console.WriteLine("st");
                        }
                        else if (direction > 0.5)
                        {
                            op = HardwareOperator.TurnRight;
                            Console.WriteLine("rt");
                        }
                        else if (direction < -0.5)
                        {
                            op = HardwareOperator.TurnLeft;
                            Console.WriteLine("lt");
                        }
                        else
                        {
                            Console.WriteLine("NOP");
                            continue; // do No Op
                        }

                        try
                        {
                            instruction = new HardwareInstruction(op, numIntervalTurn);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("[Invalid Instruction: {0}]", e.Message);
                        }
                    }
                }
            }

            catch (Exception e)
            {
                navigation.Disconnect();
                Console.WriteLine("[Invalid Instruction: {0}]", e.Message);
            }

            finally
            {
                navigation.Disconnect();
            }
         
        }
    }
}
