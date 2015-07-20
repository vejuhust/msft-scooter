using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using BingMapsRESTService.Common.JSON;

namespace NavigatorManaged
{
    public class Navigator
    {
        private ComPortReader comPort = new ComPortReader();

        private Routing routing = new Routing();

        public Coordinates currentCordinates { get; private set; }

        public string currentLocation { get; private set; }

        public void Connect()
        {
            comPort.OpenPort();
        }

        public void Disconnect()
        {
            comPort.ClosePort();
        }

        public string GetCurrentLocation()
        {
            if (comPort.IsOpen())
            {
                Stopwatch s = new Stopwatch();
                s.Start();

                while (s.Elapsed < TimeSpan.FromSeconds(150))
                {
                    var currentlocation = routing.GetCurrentLocation(comPort.Latitude.ToString(), comPort.Longitude.ToString());
                    if (!string.IsNullOrEmpty(currentlocation))
                    {
                        return currentlocation;
                    }
                }

                throw new Exception(@"Cannot find the current location, may be GPS signal is not good, try at some other location");

            }
            else
            {
                throw new EntryPointNotFoundException(@"Com port is not opend");
            }
        }

        public List<GPSInstruction.Instruction> GetRoute(string src, string des)
        {
            return routing.GetRoute(src, des);
        }

    }
}
