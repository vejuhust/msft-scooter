using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NLog;

namespace NavigatorManaged
{
    public class Logger
    {
        private static Timer aTimer;

        private readonly NLog.Logger logger = LogManager.GetCurrentClassLogger(); 


        public Logger(Navigator nav, int duration = 500)
        {
            aTimer = new Timer(500);
            aTimer.Elapsed += (sender, e) => { OnTimedEvent(nav); };
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Navigator nav)
        {
            var result = nav.LogLocation();
            if (result != null)
            {
                var lat = Math.Round(double.Parse(result.Item1), 4);
                var lon = Math.Round(double.Parse(result.Item2), 4);
                var speed = Math.Round(double.Parse(result.Item3), 4);
                var data = string.Format(@"Date:{0}, Time:{1}, Latitude:{2}, Longitude:{3}, CurrentSpeed:{4} km/h", DateTime.Now.Date, DateTime.Now.ToString("h:mm:ss tt"), lat, lon, speed);
                var data1 = string.Format(@"{0}, {1}, {2}, {3}, {4}", DateTime.Now.Date, DateTime.Now.ToString("h:mm:ss tt"), nav.getLatitude(), nav.getLongitude(), lat, lon, speed);
                Console.WriteLine(data);
                logger.Info(data1);
            }
            
        }


    }
}
