using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace NavigatorManaged
{
    public struct GpsSpeed
    {
        public double Knots;
        public double KilometersPerHour;
        public double MilesPerHour;

        public GpsSpeed(string ReportedSpeed)
        {
            Knots = 0;
            KilometersPerHour = 0;
            MilesPerHour = 0;

            if (ReportedSpeed.Trim() != "")
            {
                try
                {
                    Knots = double.Parse(ReportedSpeed, new CultureInfo("en-US"));
                    KilometersPerHour = Knots * 1.85185;
                    MilesPerHour = Knots * 1.150779;
                }
                catch (Exception ex)
                {
                    // Error parsing speed.
                }
            }
        }

        public override string ToString()
        {
            return KilometersPerHour.ToString();
        }
    }
}
