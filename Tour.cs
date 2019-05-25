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
            
            _orderedStations = new List<Station>();
            _tourTime = 0;
            _plane = plane;

            var programStopwatch = System.Diagnostics.Stopwatch.StartNew(); // start stop watch

            // optimise with level 2
            if (stations.Count() > 12)
            {
                Console.WriteLine("Optimising tour length: Level 2...");
                _levelSelected = "2";
                SimpleHeuristic(stations);
                ImprovedHeuristic();
            }
            else // optimise with level 3
            {
                Console.WriteLine("Optimising tour length: Level 3...");
                _levelSelected = "3";
                ExhaustiveSearch(stations);
            }

            programStopwatch.Stop(); // stop stopwatch

            AddStamps(initialTimeMinutes); 
            _elapsedTimeMS = (double)programStopwatch.ElapsedMilliseconds; // save elapsed time
        }

        // add time and refuel stamps for ordered stations
        public void AddStamps(double initalTimeMinutes)
        {
            double startTime; // start time of trip
            double endTime = initalTimeMinutes; // end time of trip
            foreach (Station visitingStation in _orderedStations)
            {
                double travelTime = visitingStation.TravelTime;
                double rangeToTravel = visitingStation.RangeTravelled;
                startTime = endTime; // have start time the  previous end time
                // refuel plane if cannot make next trip
                if (_plane.Range - rangeToTravel + 0.00001 < 0)
                {
                    _plane.RefuelPlane();
                    visitingStation.Refuel();
                    startTime += _plane.RefuelTime;
                }
                _plane.Range -= rangeToTravel;
                endTime = startTime + travelTime;
                visitingStation.AddTripTimes(startTime, endTime);
            }
            // save tour time as difference of last end time and first start time
            _tourTime = endTime - initalTimeMinutes; 
        }

        // Level 1 Tour
        public void SimpleHeuristic(List<Station> stations)
        {

            _orderedStations.Add(stations[0]); // first station (post office)
            _orderedStations.Add(stations[0]); // last station (post office)
            
            // loop through every station of mail
            for (int n = 0; n < stations.Count()-1; n++)
            {
                Station selectStation = stations[n + 1]; // station to be inserted

                // calculate length of tour when station is insert at N+1 (end)
                double disPrevBefore = _orderedStations[n].Distance;
                double disPrevAdjusted = TourMath.DistanceBetweenStations(_orderedStations[n], selectStation);
                double disToNext = TourMath.DistanceBetweenStations(selectStation, _orderedStations[0]);
                double minLength = _tourLength - disPrevBefore + disPrevAdjusted + disToNext;
                int bestPosition = n + 1; // position for insert, if in fact shortest length

                // loop through positions up to N+1
                for (int i = 1; i < n + 1; i++)
                {
                    // calculate length of tour when station is inserted at each position
                    disPrevBefore = _orderedStations[i - 1].Distance;
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

                _tourLength = minLength; // update tour length
            }
            // remove post office from end
            _orderedStations.RemoveAt(_orderedStations.Count() - 1);
            
        } 

        // Level 2 Tour
        public void ImprovedHeuristic()
        {
            bool changed = true; // flag for if change has been made
            _orderedStations.Add(_orderedStations[0]); // last station (post office) 

            while (changed) // loop until no more changes have occured
            {
                changed = false;
                
                // loop through all stations
                for (int i = 1; i < _orderedStations.Count()-1; i++)
                {
                    Station selectStation = _orderedStations[i];
                    int bestPosition = i;

                    double preNewLength = _tourLength; // updates to length pior insertion
                    
                    // update preNewLength and update stations around where select station is removed
                    preNewLength -= selectStation.Distance;
                    _orderedStations.RemoveAt(i); // remove
                    preNewLength -= _orderedStations[i - 1].Distance;
                    _orderedStations[i - 1].NextTo(_orderedStations[i]);
                    preNewLength += _orderedStations[i - 1].Distance;

                    // loop through positions to insert
                    for (int n = 1; n < _orderedStations.Count(); n++)
                    {
                        double newLength = preNewLength;

                        // update distances around where station would be inserted
                        double distAhead = TourMath.DistanceBetweenStations(selectStation, _orderedStations[n]);
                        newLength += distAhead;
                        double prevDistBehind = TourMath.DistanceBetweenStations(_orderedStations[n - 1], _orderedStations[n]);
                        newLength -= prevDistBehind;
                        double currentDistBehind = TourMath.DistanceBetweenStations(_orderedStations[n - 1], selectStation);
                        newLength += currentDistBehind; // potential new length
                        
                        // if new length is shorter, update best position and update tour length
                        if (newLength < _tourLength - 0.000001)
                        {
                            bestPosition = n;
                            _tourLength = newLength;
                            changed = true;
                        }
                    }

                    // insert at best position and update stations around it
                    _orderedStations[bestPosition - 1].NextTo(selectStation);
                    selectStation.NextTo(_orderedStations[bestPosition]);
                    _orderedStations.Insert(bestPosition, selectStation);
                }
            }
            // remove post office from end
            _orderedStations.RemoveAt(_orderedStations.Count() - 1);
        }

        // Level 3
        public void ExhaustiveSearch(List<Station> stationsToOrder)
        {
            // list for different permutations
            List<List<Station>> stationsPerms = new List<List<Station>>();
            // pairing list for lengths of each perm
            List<double> stationsPermsLength = new List<double>();
            
            // permutation algorithm
            void Generate(int k, List<Station> stations)
            {
                if (k == 1)
                {
                    List<Station> stationsToAdd = new List<Station>(stations);
                    stationsToAdd.Insert(0, stationsToOrder[0]);
                    stationsToAdd.Add(stationsToOrder[0]);
                    stationsPermsLength.Add(TourMath.CalculateDistancesAndFindLength(ref stationsToAdd));
                    stationsPerms.Add(stationsToAdd);
                }
                else
                {
                    Generate(k - 1, stations);
                    for (int i = 0; i < k - 1; i++)
                    {
                        if (k % 2 == 0)
                        {
                            Station temp1 = stations[i];
                            Station temp2 = stations[k - 1];
                            TourMath.SwapStations(ref temp1, ref temp2);
                            stations[i] = temp1;
                            stations[k - 1] = temp2;
                        }
                        else
                        {
                            Station temp1 = stations[0];
                            Station temp2 = stations[k - 1];
                            TourMath.SwapStations(ref temp1, ref temp2);
                            stations[0] = temp1;
                            stations[k - 1] = temp2;
                        }
                        Generate(k - 1, stations);
                    }
                }
            }
            // run permutation algorithm
            Generate(stationsToOrder.Count()-1, stationsToOrder.GetRange(1,stationsToOrder.Count()-1));

            // get shortest length and its index in list
            double minLength = stationsPermsLength.Min();
            int indexOfMin = stationsPermsLength.IndexOf(minLength);

            // get tour of shortest length and clean it (see TourMath)
            List<Station> shortestTour = stationsPerms[indexOfMin];
            TourMath.CleanUpPermutation(ref shortestTour);
            shortestTour.RemoveAt(shortestTour.Count() - 1);
            
            _orderedStations = new List<Station>(shortestTour);
            _tourLength = minLength;
        }
        
        // methods to return tour properties
        public List<Station> OrderedStations { get { return _orderedStations; } }
        public double TourLength { get { return _tourLength; } }
        public double TourTime { get { return _tourTime; } }
        public string LevelSelected { get { return _levelSelected; } }
        public double ElapsedTimeMS { get { return _elapsedTimeMS; } }

    }
}
