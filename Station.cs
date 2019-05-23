using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flying_Postman
{
    class Station
    {
        private string _name;
        private int _x;
        private int _y;
        private string _nextStation;
        private double _distance;
        private double _travelTime;
        private double _rangeTravelled;
        private bool _refuels;
        private double _tripStartTime;
        private double _tripEndTime;
        private Plane _plane;

        public Station(string name, int x, int y, Plane plane)
        {
            _name = name;
            _x = x;
            _y = y;
            _nextStation = "";
            _distance = 0;
            _travelTime = 0;
            _rangeTravelled = 0;
            _refuels = false;
            _tripStartTime = 0;
            _tripEndTime = 0;
            _plane = plane;
        }

        public void NextTo(Station nextStation)
        {
            _nextStation = nextStation.Name;
            _distance = TourMath.DistanceBetween(_x, _y, nextStation.X, nextStation.Y);
            _travelTime = TourMath.CalculateTravelTime(_distance, _plane);
            _rangeTravelled = _travelTime / 60;
        }

        public void AddTripTimes(double startTime, double endTime)
        {
            _tripStartTime = startTime;
            _tripEndTime = endTime;
        }

        public void Refuel() { _refuels = true; }

        public string Name { get { return _name; } }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public string NextStation { get { return _nextStation; } }
        public double Distance { get { return _distance; } }
        public double TravelTime { get { return _travelTime; } }
        public double RangeTravelled { get { return _rangeTravelled; } }
        public bool Refuels { get { return _refuels; } }
        public double TripStartTime { get { return _tripStartTime; } }
        public double TripEndTime { get { return _tripEndTime; } }
        public string TripStartTimeStamp
        {
            get { return TourMath.ConvertToTimeStamp(_tripStartTime); }
        }
        public string TripEndTimeStamp
        {
            get { return TourMath.ConvertToTimeStamp(_tripEndTime); }
        }
        

    }
    
}
