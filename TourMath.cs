using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flying_Postman
{
    struct TourMath
    {
        
        public static double DistanceBetween(int x0, int y0, int x1, int y1)
        {
            double distance = Math.Sqrt(Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2));
            return distance;
        }

        public static double DistanceBetweenStations(Station firstStation, Station secondStation)
        {
            int x0 = firstStation.X;
            int y0 = firstStation.Y;
            int x1 = secondStation.X;
            int y1 = secondStation.Y;
            double distance = DistanceBetween(x0, y0, x1, y1);
            return distance;
        }

        public static double CalculateTravelTime(double distance, Plane plane)
        {
            double speed = plane.Speed;
            double takeOffTime = plane.TakeOffTime;
            double landingTime = plane.LandingTime;
            double travelTime = ((distance / speed) * 60 + takeOffTime + landingTime);
            return Math.Round(travelTime);
        }


        public static double CalculateCurrentTime(double previousTime, double travelTime)
        {
            return previousTime + travelTime;
        }

        public static string ConvertToTimeStamp(double timeInMinutes)
        {

            int hours = (int)(timeInMinutes / 60);
            int minutes = (int)(timeInMinutes % 60);

            if (hours >= 24) { hours = hours % 24; }

            string stringHours = hours.ToString();
            if (hours < 10) { stringHours = "0" + stringHours; }

            string stringMinutes = minutes.ToString();
            if (minutes < 10) { stringMinutes = "0" + stringMinutes; }

            return stringHours + ":" + stringMinutes;
        }

        public static int ConvertToTimeMinutes(string timeStamp)
        {
            string stringHours = timeStamp.Substring(0, 2);
            string stringMinutes = timeStamp.Substring(3);

            int hours = int.Parse(stringHours);
            int minutes = int.Parse(stringMinutes);

            int totalMinutes = hours * 60 + minutes;
            return totalMinutes;
        }

        public static double CalculateSumDistances(List<Station> stations)
        {
            double sum = 0;
            foreach (Station station in stations)
            {
                sum += station.Distance;
            }
            return sum;
        }
        
    }
    
}
