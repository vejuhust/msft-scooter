using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace NavigatorManaged
{

    public struct LatitudeLongitude
    {
        public int Degrees;
        public double Minutes;
        public string Indicator;
        public string DegreeString;

        public LatitudeLongitude(string LatitudeString, string Indicator)
        {
            string indicator = Indicator.ToUpper();
            DegreeString = @"";
            CultureInfo cinfo = new CultureInfo("en-US");

            switch (indicator)
            {
                case "E":
                    {
                        Degrees = int.Parse(LatitudeString.Substring(0, 3), cinfo);
                        Minutes = double.Parse(LatitudeString.Substring(3), cinfo);
                        break;
                    }
                case "W":
                    {
                        Degrees = int.Parse(LatitudeString.Substring(0, 3), cinfo);
                        Minutes = double.Parse(LatitudeString.Substring(3), cinfo);
                        break;
                    }
                case "N":
                    {
                        Degrees = int.Parse(LatitudeString.Substring(0, 2), cinfo);
                        Minutes = double.Parse(LatitudeString.Substring(2), cinfo);
                        break;
                    }
                case "S":
                    {
                        Degrees = int.Parse(LatitudeString.Substring(0, 2), cinfo);
                        Minutes = double.Parse(LatitudeString.Substring(2), cinfo);
                        break;
                    }
                default:
                    {
                        Degrees = 0;
                        Minutes = 0;
                        break;
                    }
            }

            this.Indicator = Indicator;
            this.DegreeString = GetDegreeString();
        }

        public string GetDegreeString()
        {
            var degrees = Degrees + (Minutes / 60f);
            string indicator = Indicator.ToUpper();
            string value = @"";

            switch (indicator)
            {
                case "N":
                case "E":
                    {
                        break;
                    }
                case "S":
                case "W":
                    {
                        value += @"-";
                        break;
                    }
            }
            value += degrees.ToString();
            return value;
        }

        public override string ToString()
        {
            return DegreeString;
        }

    }

    public struct Coordinates
    {
        private string coordinates;

        public Coordinates(LatitudeLongitude latitude, LatitudeLongitude longitude)
        {
            if (latitude.Degrees != 0 || latitude.Degrees != 0)
            {
                coordinates = latitude.ToString() + @"," + longitude.ToString();
            }
            else
            {
                coordinates = null;
            }
        }

        public override string ToString()
        {
            return coordinates;
        }

    }
}
