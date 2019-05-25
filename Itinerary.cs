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
        // reference
        private Tour _tour;
        private Plane _plane;
        
        private List<string> _itineraryStartLog; // log of messeges before trip is shown
        private List<string> _tourTripLog; // log of trip
        
        public Itinerary(Tour tour, Plane plane)
        {
            _tour = tour; 
            _plane = plane;

            _itineraryStartLog = new List<string>();
            _tourTripLog = new List<string>();
        
            string startingLine = "Optimising tour length: Level " + tour.LevelSelected + "...";
            _itineraryStartLog.Add(startingLine);

            // add tour details to itinerary logs
            AddElapsedTimeLog(_tour.ElapsedTimeMS);
            AddTourTimeLog(_tour.TourTime);
            AddTourLengthLog(_tour.TourLength);
            AddTourTripsLog(_tour.OrderedStations); // trip
        }
        
        // Print itinerary logs to console
        public void PrintItinerary()
        {
            foreach (string log in _itineraryStartLog.GetRange(1,_itineraryStartLog.Count()-1))
            {
                Console.WriteLine(log);
            }
            foreach (string log in _tourTripLog)
            {
                Console.WriteLine(log);
            }
        }

        // save itinerary logs to out file
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

        // add elapsed time in ms to starting log as seconds
        public void AddElapsedTimeLog(double elapsedMilliseconds)
        {
            double elapsedSeconds = elapsedMilliseconds / 1000;
            string log = "Elapsed time: " + elapsedSeconds.ToString() + " seconds";
            _itineraryStartLog.Add(log);
        }

        // add the tour time in minutes to starting log as days, hours, minutes
        public void AddTourTimeLog(double tourTime)
        {
            string days = Convert.ToString((int)(tourTime / 60 / 24));
            string hours = Convert.ToString((int)(tourTime / 60 % 24));
            string minutes = Convert.ToString((int)(tourTime % 60));
            string log = "Tour time: " + days + " days " + hours + " hours " + minutes + " minutes";
            _itineraryStartLog.Add(log);
        }

        // add tour length (total distance) to starting log
        public void AddTourLengthLog(double tourLength)
        {
            tourLength = Math.Round(tourLength, 3);
            string log = "Tour length: " + Convert.ToString(tourLength);
            _itineraryStartLog.Add(log);
        }

        // add each trip of the tour to trip log
        public void AddTourTripsLog(List<Station> orderedStations)
        {
            // loop through stations to get properties for log
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

        // add a refuel log to trip log
        public void AddRefuelLog(double refuelTime)
        {
            string log = "*** refuel in " + Convert.ToString(refuelTime) + " minutes ***";
            _tourTripLog.Add(log);
        }
        

    }
}
