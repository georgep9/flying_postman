using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flying_Postman
{
    class Tour
    {

        private List<Station> _orderedStations;
        private double _tourLength;
        private double _tourTime;
        private string _levelSelected;
        private double _elapsedTimeMS;
        private Plane _plane;
        
        public Tour(List<Station> stations, Plane plane, double initialTimeMinutes)
        {
            var programStopwatch = System.Diagnostics.Stopwatch.StartNew();

            _orderedStations = new List<Station>();
            _tourTime = 0;
            _plane = plane;

            _levelSelected = "1";
            SimpleHeuristic(plane, stations);
            programStopwatch.Stop();

            AddStamps(initialTimeMinutes);
            
            _elapsedTimeMS = (double)programStopwatch.ElapsedMilliseconds;
        }

        public void AddStamps(double initalTimeMinutes)
        {
            double startTime;
            double endTime = initalTimeMinutes;
            foreach (Station visitingStation in _orderedStations)
            {
                double travelTime = visitingStation.TravelTime;
                double rangeToTravel = visitingStation.RangeTravelled;
                startTime = endTime;
                if (Math.Round((_plane.Range - rangeToTravel),7) < 0)
                {
                    if (visitingStation.Name == "QFB")
                    {
                        Console.WriteLine(" {0} {1}", (int)(_plane.Range * 60), (int)(rangeToTravel * 60));
                        Console.WriteLine(" {0}", (int)(_plane.Range * 60) - (int)(rangeToTravel * 60));
                    }
                    _plane.RefuelPlane();
                    visitingStation.Refuel();
                    startTime += _plane.RefuelTime;
                }
                _plane.Range -= rangeToTravel;
                endTime = startTime + travelTime;
                visitingStation.AddTripTimes(startTime, endTime);
            }
            _tourTime = endTime - initalTimeMinutes;
        }

        // Level 1 Tour
        public void SimpleHeuristic(Plane plane, List<Station> stations)
        {

            _orderedStations.Add(stations[0]); // first station (post office)
            _orderedStations.Add(stations[0]); // last station (post office)
            
            // loop through every station of mail
            for (int n = 0; n < stations.Count() - 1; n++)
            {
                Station selectStation = stations[n + 1]; // station to be inserted

                // calculate length of tour when station is insert at N+1 (end)
                double disPrevBefore = TourMath.DistanceBetweenStations(_orderedStations[n], _orderedStations[n+1]);
                double disPrevAdjusted = TourMath.DistanceBetweenStations(_orderedStations[n], selectStation);
                double disToNext = TourMath.DistanceBetweenStations(selectStation, _orderedStations[0]);
                double minLength = _tourLength - disPrevBefore + disPrevAdjusted + disToNext;
                int bestPosition = n + 1; // position for insert, if in fact shortest length

                // loop through positions up to N+1
                for (int i = 1; i < n + 1; i++)
                {
                    // calculate length of tour when station is inserted at each position

                    disPrevBefore = TourMath.DistanceBetweenStations(_orderedStations[i - 1], _orderedStations[i]);
                    disPrevAdjusted = TourMath.DistanceBetweenStations(_orderedStations[i - 1], selectStation);
                    disToNext = TourMath.DistanceBetweenStations(selectStation, _orderedStations[i]);
                    double newLength = _tourLength - disPrevBefore + disPrevAdjusted + disToNext;
                    // if this length is smaller than the current shortest
                    if (newLength < minLength)
                    {
                        minLength = newLength; // new shortest length
                        bestPosition = i; // current best position for insert
                    }
                }
                // insert station at best recorded position
                _orderedStations.Insert(bestPosition, selectStation);
                // update stations around it
                _orderedStations[bestPosition - 1].NextTo(selectStation);
                _orderedStations[bestPosition].NextTo(_orderedStations[bestPosition + 1]);

                _tourLength = minLength;
            }
            // remove post office from end
            _orderedStations.RemoveAt(_orderedStations.Count() - 1);
            
        } 
        
        public List<Station> OrderedStations { get { return _orderedStations; } }
        public double TourLength { get { return _tourLength; } }
        public double TourTime { get { return _tourTime; } }
        public string LevelSelected { get { return _levelSelected; } }
        public double ElapsedTimeMS { get { return _elapsedTimeMS; } }

    }
}
