using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingMapsRESTService.Common.JSON;

namespace NavigatorManaged
{
    public class GPSInstruction
    {
        public struct Instruction
        {
            public double Startlatitude { get; set; }

            public double StartLongitude { get; set; }

            public double Finishlatitude { get; set; }

            public double FinishLongitude { get; set; }

            public double Distance { get; set; }

            public string[] RoadName { get; set; }

            public string NextRoadName { get; set; }

            public TurnType Action { get; set; }

            public string TravelMode { get; set; }

            public string CompassDirection { get; set; }

            public string RoadType { get; set; }
        }

        public List<Instruction> InstructionList { get; private set; }

        public GPSInstruction (ItineraryItem[] items)
        {
            InstructionList = new List<Instruction>();
            var totalCount = items.Length;
            for (var currentItemCount = 0; currentItemCount < totalCount-1; currentItemCount++)
            {
                var instruction = new Instruction();
                instruction.Startlatitude = items[currentItemCount].ManeuverPoint.Coordinates[0];
                instruction.StartLongitude = items[currentItemCount].ManeuverPoint.Coordinates[1];
                instruction.Finishlatitude = items[currentItemCount + 1].ManeuverPoint.Coordinates[0];
                instruction.FinishLongitude = items[currentItemCount + 1].ManeuverPoint.Coordinates[1];
                instruction.Distance = items[currentItemCount].TravelDistance;
                if (items[currentItemCount].Details != null)
                {
                    instruction.RoadName = items[currentItemCount].Details[0].Names;
                }
                
                instruction.NextRoadName = items[currentItemCount].Instruction.Text;
                switch (items[currentItemCount].Instruction.ManeuverType)
                {
                    case "TurnRight":
                        {
                            instruction.Action = TurnType.TurnRight;
                            break;
                        }
                    case "TurnLeft":
                        {
                            instruction.Action = TurnType.TurnLeft;
                            break;
                        }
                    default:
                        {
                            instruction.Action = TurnType.KeepStraight;
                            break;
                        }
                }

                instruction.TravelMode = items[currentItemCount].TravelMode;
                instruction.CompassDirection = items[currentItemCount].CompassDirection;
                InstructionList.Add(instruction);
            }
           

        }
    }
}
