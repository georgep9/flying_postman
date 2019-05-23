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
        private List<double> _distancesToNext;
        private List<StationPair> _orderedStationPairs;
        private double _tourTime;
        private double _tourLength;

        Itinerary itinerary;
        
        public Tour(List<Station> stations, Plane plane, double initialTimeMinutes)
        {
            var programStopwatch = System.Diagnostics.Stopwatch.StartNew();

            _orderedStations = new List<Station>();
            _orderedStationPairs = new List<StationPair>();

            _distancesToNext = new List<double>(new double[stations.Count()+1]);

            _tourTime = 0;
            _tourLength = 0;
            
            itinerary = new Itinerary("1");
            
            SimpleHeuristic(plane, stations);

            double previousTime = initialTimeMinutes;
            int amountOfStations = _orderedStations.Count();

            for (int n = 1; n < amountOfStations; n++)
            {
                Station previousStation = _orderedStations[n - 1];
                Station newStation = _orderedStations[n];

                CompareStations compareStations = new CompareStations(previousStation, newStation, plane);
                double distance = compareStations.Distance;
                double travelTime = compareStations.TravelTime;
                double rangeTravelled = compareStations.RangeTravelled;
                
                _tourTime += travelTime;
                plane.Range -= rangeTravelled;
                if (plane.Range <= 0)
                {
                    previousTime += plane.RefuelTime;
                    _tourTime += plane.RefuelTime;
                    plane.RefuelPlane();
                    itinerary.AddRefuelLog(plane.RefuelTime);
                }

                double currentTime = TourMath.CalculateCurrentTime(previousTime, travelTime);


                string currentTimeStamp = TourMath.ConvertToTimeStamp(currentTime);
                string previousTimeStamp = TourMath.ConvertToTimeStamp(previousTime);

                itinerary.AddTripLog(previousStation.StationName, newStation.StationName, previousTimeStamp, currentTimeStamp);

                previousTime = currentTime;
            }

            programStopwatch.Stop();
            double elapsedTime = (double)programStopwatch.ElapsedMilliseconds;
            itinerary.AddElapsedTimeLog(elapsedTime);
            itinerary.AddTourTimeLog(_tourTime);
            itinerary.AddTourLengthLog(_tourLength);
            itinerary.PrintItinerary();

            /*SimpleHeuristicFixed(stations, plane);
            foreach (StationPair pair in _orderedStationPairs)
            {
                Console.WriteLine("{0}\t{1}", pair.FirstStation.StationName, pair.SecondStation.StationName);
            }*/
            
        }

        // Level 1 Tour
        public void SimpleHeuristic(Plane plane, List<Station> stations)
        {

            _orderedStations.Add(stations[0]); // first station (post office)
            _orderedStations.Add(stations[0]); // last station

            double sumDistancesBefore;
            double sumDistancesAfter;
            double distanceBefore;
            double distanceAfter;

            double bestDistBefore;
            double bestDistAfter;

            // loop through every station of mail
            for (int n = 0; n < stations.Count() - 1; n++)
            {
                Station selectStation = stations[n + 1]; // N+1 station

                // set shortest tour length to be tour with N+1 station added at the end
                sumDistancesBefore = TourMath.CalculateSumDistances(_distancesToNext.GetRange(0, n));
                bestDistBefore = TourMath.CalculateDistance(_orderedStations[n], selectStation);
                bestDistAfter = TourMath.CalculateDistance(selectStation, _orderedStations[0]);
                double minLength = sumDistancesBefore + bestDistBefore + bestDistAfter; // length of tour
                int bestPosition = n + 1; // position for insert

                // loop through ordered stations upto N
                for (int i = 1; i < n + 1; i++)
                {
                    // insert N+1 station to check for impact
                    sumDistancesBefore = TourMath.CalculateSumDistances(_distancesToNext.GetRange(0, i - 1));
                    sumDistancesAfter = TourMath.CalculateSumDistances(_distancesToNext.GetRange(i, n + 1 - i));
                    distanceBefore = TourMath.CalculateDistance(_orderedStations[i - 1], selectStation);
                    distanceAfter = TourMath.CalculateDistance(selectStation, _orderedStations[i]);

                    double newLength = sumDistancesBefore + distanceBefore + distanceAfter + sumDistancesAfter;
                    // if this length is smaller than the current shortest
                    if (newLength < minLength)
                    {
                        bestDistBefore = distanceBefore;
                        bestDistAfter = distanceAfter;
                        minLength = newLength; // new shortest length
                        bestPosition = i; // new position for insert
                    }
                }
                // insert N+1 station at best recorded position
                _orderedStations.Insert(bestPosition, selectStation);
                _distancesToNext[bestPosition - 1] = bestDistBefore;
                _distancesToNext[bestPosition] = bestDistAfter;

                _tourLength = minLength;
            }

        } 

        /*public void SimpleHeuristicFixed(List<Station> stations, Plane plane)
        {
            _orderedStationPairs.Add(new StationPair(stations[0], stations[0], plane));
            _orderedStationPairs.Add(new StationPair(stations[0], stations[0], plane));
            
            List<StationPair> tempPairs;
            StationPair newPair;
            StationPair bestPair;
            double minLength;
            int bestPosition;
            for (int n=0; n < stations.Count()-1; n++)
            {
                Station selectStation = stations[n + 1];

                tempPairs = new List<StationPair>(_orderedStationPairs);
                newPair = new StationPair(selectStation, stations[0], plane);
                tempPairs.Insert(n + 1, newPair);
                tempPairs[n] = new StationPair(_orderedStationPairs[n].FirstStation, selectStation, plane);
                bestPair = new StationPair(selectStation, stations[0], plane);
                minLength = TourMath.CalculateTourLength(tempPairs);
                bestPosition = n + 1;

                for (int i = 1; i<n+1; i++)
                {
                    tempPairs = new List<StationPair>(_orderedStationPairs);
                    newPair = new StationPair(selectStation, _orderedStationPairs[i].FirstStation, plane);

                    tempPairs.Insert(i, newPair);
                    tempPairs[i - 1] = new StationPair(_orderedStationPairs[i - 1].FirstStation, selectStation, plane);
                    tempPairs[i + 1] = new StationPair(selectStation, _orderedStationPairs[i + 1].FirstStation, plane);

                    double newLength = TourMath.CalculateTourLength(tempPairs);
                    if (newLength < minLength)
                    {
                        bestPair = new StationPair(selectStation, _orderedStationPairs[i].FirstStation, plane);
                        bestPosition = i;
                        minLength = newLength;
                    }
                }

                _orderedStationPairs.Insert(bestPosition, bestPair);

            }*/
            

            /*for (int n = 0; n < stations.Count() - 1; n++)
            {
                newStation = stations[n+1];
                _orderedStationPairs.Add(new StationPair(newStation, stations[0],plane));
                newPairs[n].ChangeSecond(newStation);
                minLength = TourMath.CalculateTourLength(newPairs);
                bestPosition = n;

                for (int i = 1; i < n+1; i++)
                {
                    tempPairs = new List<StationPair>(_orderedStationPairs);
                    tempPairs.Insert(i, new StationPair(newStation, tempPairs[i-1].SecondStation, plane));
                    tempPairs[i-1].ChangeSecond(newStation);
                    newLength = TourMath.CalculateTourLength(tempPairs);
                    if (newLength < minLength)
                    {
                        minLength = newLength;
                        bestPosition = i;
                    }
                }
                _orderedStationPairs = new List<StationPair>(newPairs);
                Console.WriteLine("{0}", bestPosition);
            }*/
        

        public Itinerary Itinerary { get { return itinerary; } }

    }
}
