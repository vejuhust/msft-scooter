using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigatorManaged
{
    public enum TurnType
    {
        KeepStraight,
        TurnRight,
        TurnLeft
        
    }
    public class Helper
    {
        /// <summary>
        /// Calculate the distance between two points
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public double distance(double lat1, double lon1, double lat2, double lon2, char unit) 
        {
          double theta = lon1 - lon2;
          double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
          dist = Math.Acos(dist);
          dist = rad2deg(dist);
          dist = dist * 60 * 1.1515;
          if (unit == 'K') 
          {
            dist = dist * 1.609344;
          } 
          else if (unit == 'N') 
          {
  	        dist = dist * 0.8684;
          }

          return (dist);
        }


        /// <summary>
        /// This function converts decimal degrees to radians
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>

        public double deg2rad(double deg) 
        {
          return (deg * Math.PI / 180.0);
        }

        /// <summary>
        /// This function converts radians to decimal degrees 
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public double rad2deg(double rad) 
        {
          return (rad / Math.PI * 180.0);
        }
    }
}
