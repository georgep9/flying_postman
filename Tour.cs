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
        private double _tourTime;
        private double _tourLength;

        private Plane _plane;

        Itinerary itinerary;
        
        public Tour(List<Station> stations, Plane plane, double initialTimeMinutes)
        {
            var programStopwatch = System.Diagnostics.Stopwatch.StartNew();

            _orderedStations = new List<Station>();
            _tourTime = 0;
            _tourLength = 0;
            _plane = plane;
            
            itinerary = new Itinerary("1");

            double range = plane.Range;
            int speed = plane.Speed;
            int takeOffTime = plane.TakeOffTime;
            int landingTime = plane.LandingTime;
            int refuelTime = plane.RefuelTime;

            double previousTime = initialTimeMinutes;

            int amountOfStations = stations.Count();

            for (int n = 1; n < amountOfStations; n++)
            {

                string previousStation = stations[n - 1].StationName();
                string newStation = stations[n].StationName();

                CompareStations compareStations = new CompareStations(stations[n - 1], stations[n],plane);

                double distance = compareStations.Distance;
                _tourLength += distance;
                double travelTime = compareStations.TravelTime;
                _tourTime += travelTime;

                double rangeTravelled = compareStations.RangeTravelled;
                plane.Range -= rangeTravelled;
                if (plane.Range <= 0)
                {
                    previousTime += refuelTime;
                    _tourTime += refuelTime;
                    plane.RefuelPlane();
                    itinerary.AddRefuelLog(refuelTime);
                }

                double currentTime = TourMath.CalculateCurrentTime(previousTime, travelTime);


                string currentTimeStamp = TourMath.ConvertToTimeStamp(currentTime);
                string previousTimeStamp = TourMath.ConvertToTimeStamp(previousTime);

                itinerary.AddTripLog(previousStation, newStation, previousTimeStamp, currentTimeStamp);

                previousTime = currentTime;
            }

            programStopwatch.Stop();
            long elapsedTime = programStopwatch.ElapsedMilliseconds / 1000;

            itinerary.AddElapsedTimeLog(elapsedTime);
            itinerary.AddTourTimeLog(_tourTime);
            itinerary.AddTourLengthLog(_tourLength);
            itinerary.PrintItinerary();

            SimpleHeuristic(plane, stations);
            foreach (Station station in _orderedStations)
            {
                Console.WriteLine(station.StationName());
            }
            
        }

        public Itinerary ReturnItinerary()
        {
            return itinerary;
        }

        // Level 1 Tour
        public void SimpleHeuristic(Plane plane, List<Station> stations)
        {
            Station selectStation; // for N+1 station
            List<Station> tempStations; // temporary list of ordered stations used to measure impact
            double minLength; // conditional value for shortest tour
            int bestPosition; // index for N+1 station to be inserted

            _orderedStations.Add(stations[0]); // first station (post office)
            _orderedStations.Add(stations[0]); // last station
            
            // loop through every station of mail
            for (int n=0; n < stations.Count()-1; n++)
            {
                selectStation = stations[n+1]; // N+1 station

                // set shortest tour length to be tour with N+1 station added at the end
                tempStations = new List<Station>(_orderedStations);
                tempStations.Insert(n + 1, selectStation);
                minLength = TourMath.CalculateTourLength(tempStations); // length of tour
                bestPosition = n + 1; // position for insert

                // loop through ordered stations upto N
                for (int i=1; i < n+1; i++)
                {
                    // insert N+1 station to check for impact
                    tempStations = new List<Station>(_orderedStations);
                    tempStations.Insert(i, selectStation);
                    double newLength = TourMath.CalculateTourLength(tempStations); // length

                    // if this length is smaller than the current shortest
                    if (newLength < minLength)
                    {
                        bestPosition = i; // new position for insert
                        minLength = newLength; // new shortest length
                    }
                }
                // insert N+1 station at best recorded position
                _orderedStations.Insert(bestPosition, selectStation);
            }

        }
    }
}
