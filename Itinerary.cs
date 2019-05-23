using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Flying_Postman
{
    class Itinerary
    {

        private Tour _tour;
        private Plane _plane;

        private List<string> _itineraryStartLog;
        private List<string> _tourTripLog;
        
        public Itinerary(Tour tour, Plane plane)
        {
            _tour = tour;
            _plane = plane;

            _itineraryStartLog = new List<string>();
            _tourTripLog = new List<string>();
            
            string startingLine = "Optimising tour length: Level " + tour.LevelSelected + "...";
            _itineraryStartLog.Add(startingLine);
            AddElapsedTimeLog(_tour.ElapsedTimeMS);
            AddTourTimeLog(_tour.TourTime);
            AddTourLengthLog(_tour.TourLength);
            AddTourTripsLog(_tour.OrderedStations);
        }
        
        public void PrintItinerary()
        {
            foreach (string log in _itineraryStartLog)
            {
                Console.WriteLine(log);
            }
            foreach (string log in _tourTripLog)
            {
                Console.WriteLine(log);
            }
        }

        public void SaveItinerary(string fileName)
        {
            Console.WriteLine("Saving itinerary to " + fileName);

            FileStream outFile = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            foreach (string log in _itineraryStartLog)
            {
                writer.WriteLine(log);
            }
            foreach (string log in _tourTripLog)
            {
                writer.WriteLine(log);
            }
            writer.Close();
            outFile.Close();
        }

        public void AddElapsedTimeLog(double elapsedMilliseconds)
        {
            double elapsedSeconds = elapsedMilliseconds / 1000;
            string log = "Elapsed time: " + elapsedSeconds.ToString() + " seconds";
            _itineraryStartLog.Add(log);
        }

        public void AddTourTimeLog(double tourTime)
        {
            string days = Convert.ToString((int)(tourTime / 60 / 24));
            string hours = Convert.ToString((int)(tourTime / 60 % 24));
            string minutes = Convert.ToString((int)(tourTime % 60));
            string log = "Tour time: " + days + " days " + hours + " hours " + minutes + " minutes";
            _itineraryStartLog.Add(log);
        }

        public void AddTourLengthLog(double tourLength)
        {
            tourLength = Math.Round(tourLength, 3);
            string log = "Tour length: " + Convert.ToString(tourLength);
            _itineraryStartLog.Add(log);
        }

        public void AddTourTripsLog(List<Station> orderedStations)
        {
            foreach (Station station in orderedStations)
            {
                if ( station.Refuels ) { AddRefuelLog(_plane.RefuelTime); }
                string currentStation = station.Name;
                string nextStation = station.NextStation;
                string currentTimeStamp = station.TripStartTimeStamp;
                string nextTimeStamp = station.TripEndTimeStamp;
                string log = currentStation + "\t->\t" + nextStation + " \t" + currentTimeStamp + "\t" + nextTimeStamp;
                _tourTripLog.Add(log);
            }
            
        }

        public void AddRefuelLog(double refuelTime)
        {
            string log = "*** refuel in " + Convert.ToString(refuelTime) + " minutes ***";
            _tourTripLog.Add(log);
        }
        

    }
}
