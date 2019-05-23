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
        private double _distance;
        private double _travelTime;
        private double _rangeTravelled;

        public Station(string name, int x, int y)
        {
            _name = name;
            _x = x;
            _y = y;
            _distance = 0;
            _travelTime = 0;
            _rangeTravelled = 0;
        }

        public string StationName { get { return _name; } }
        public int StationX { get { return _x; } }
        public int StationY { get { return _y; } }
        public double Distance { get { return _distance; } }
        public double TravelTime { get { return _travelTime; } }
        public double RangeTravelled { get { return _rangeTravelled; } }

    }

    class StationPair
    {
        private Station _firstStation;
        private Station _secondStation;
        private Plane _plane;
        private double _distance;
        private double _travelTime;
        private double _rangeTravelled;



        public StationPair(Station firstStation, Station secondStation, Plane plane)
        {
            _firstStation = firstStation;
            _secondStation = secondStation;
            _plane = plane;
            Calculate();
        }

        private void Calculate()
        {
            _distance = TourMath.CalculateDistance(_firstStation, _secondStation);
            _travelTime = TourMath.CalculateTravelTime(_firstStation, _secondStation, _plane);
            _rangeTravelled = _travelTime / 60;
        }
        

        public Station FirstStation { get { return _firstStation; } }
        public Station SecondStation { get { return _secondStation; } }
        public double Distance { get { return _distance; } }
        public double TravelTime { get { return _travelTime; } }
        public double RangeTravelled { get { return _rangeTravelled; } }
    }
}
