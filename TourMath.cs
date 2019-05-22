using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flying_Postman
{
    class TourMath
    {
        
        public static double CalculateDistance(Station firstStation, Station secondStation)
        {
            int x0 = firstStation.StationX();
            int y0 = firstStation.StationY();
            int x1 = secondStation.StationX();
            int y1 = secondStation.StationY();
            double distance = Math.Sqrt(Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2));
            return distance;
        }

        public static double CalculateTravelTime(Station firstStation, Station secondStation, Plane plane)
        {
            double distance = CalculateDistance(firstStation, secondStation);
            int speed = plane.Speed;
            int takeOffTime = plane.TakeOffTime;
            int landingTime = plane.LandingTime;
            double travelTime = ((distance / speed) * 60 + takeOffTime + landingTime);
            return travelTime;
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

        public static double CalculateTourLength(List<Station> stations)
        {
            double tourLength = 0;
            double distanceBetween;
            for (int i = 1; i < stations.Count(); i++)
            {
                distanceBetween = CalculateDistance(stations[i - 1], stations[i]);
                tourLength += distanceBetween;
            }
            return tourLength;
        }
        
    }

    class CompareStations
    {
        private double _distance;
        private double _travelTime;
        private double _rangeTravelled;

        public CompareStations(Station firstStation, Station secondStation, Plane plane)
        {
            _distance = TourMath.CalculateDistance(firstStation, secondStation);
            _travelTime = TourMath.CalculateTravelTime(firstStation, secondStation,plane);
            _rangeTravelled = _travelTime / 60;

        }

        public double Distance { get { return _distance; } }
        public double TravelTime { get { return _travelTime; } }
        public double RangeTravelled { get { return _rangeTravelled; } }
        
    }
}
