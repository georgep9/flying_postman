using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flying_Postman
{
    struct TourMath
    {
        // calculate distance between two coordinates
        public static double DistanceBetween(int x0, int y0, int x1, int y1)
        {
            double distance = Math.Sqrt(Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2));
            return distance;
        }

        // calulate distance between stations from their properties
        public static double DistanceBetweenStations(Station firstStation, Station secondStation)
        {
            int x0 = firstStation.X;
            int y0 = firstStation.Y;
            int x1 = secondStation.X;
            int y1 = secondStation.Y;
            double distance = DistanceBetween(x0, y0, x1, y1);
            return distance;
        }

        // calculate travel time from distance travelled, with taking off and landing times
        public static double CalculateTravelTime(double distance, Plane plane)
        {
            double speed = plane.Speed;
            double takeOffTime = plane.TakeOffTime;
            double landingTime = plane.LandingTime;
            double travelTime = ((distance / speed) * 60 + takeOffTime + landingTime);
            return Math.Round(travelTime);
        }

        // convert time in minutes to time stamp, as 24 hour time
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

        // convert 24 hour time stamp string to minutes to minutes in
        public static int ConvertToTimeMinutes(string timeStamp)
        {
            string stringHours = timeStamp.Substring(0, 2);
            string stringMinutes = timeStamp.Substring(3, 2);
            int hours = int.Parse(stringHours);
            int minutes = int.Parse(stringMinutes);
            int totalMinutes = hours * 60 + minutes;
            return totalMinutes;
        }

        // used for Exhaustive search, assigns next station to each station and sums
        // the distances accordingly
        public static double CalculateDistancesAndFindLength(ref List<Station> stations)
        {
            double length = 0;

            for (int i = 0; i < stations.Count()-1; i++)
            {
                stations[i].NextTo(stations[i + 1]);
                length += stations[i].Distance;
            }
            stations[stations.Count() - 1].NextTo(stations[1]);

            return length;
        }

        // fix properties of each station in a stations list permutation
        public static void CleanUpPermutation(ref List<Station> stations)
        {
            for (int i = 0; i < stations.Count()-1; i++)
            {
                stations[i].NextTo(stations[i + 1]);
            }
        }

        // swap stations
        public static void SwapStations(ref Station station1, ref Station station2)
        {
            Station temp = new Station(station1.Name, station1.X, station1.Y, station1.Plane);
            station1 = new Station(station2.Name, station2.X, station2.Y, station2.Plane);
            station2 = temp;
        }

    }
    
}
