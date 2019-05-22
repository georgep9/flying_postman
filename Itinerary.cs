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

        private List<string> _itineraryStartLog;
        private List<string> _tourTripLog;

        public Itinerary(string level)
        {
            _itineraryStartLog = new List<string>();
            _tourTripLog = new List<string>();
            string startingLine = "Optimising tour length: Level " + level;
            _itineraryStartLog.Add(startingLine);
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

        public void AddElapsedTimeLog(long elapsedTime)
        {
            string log = "Elapsed time: " + elapsedTime.ToString() + " seconds.";
            _itineraryStartLog.Add(log);
        }

        public void AddTourTimeLog(double tourTime)
        {
            int hours = (int)(tourTime / 60);
            int minutes = (int)(tourTime % 60);
            string log = "Tour time: " + Convert.ToString(hours) + " Hours " + Convert.ToString(minutes) + " Minutes.";
            _itineraryStartLog.Add(log);
        }

        public void AddTourLengthLog(double tourLength)
        {
            string log = "Tour length: " + Convert.ToString(tourLength);
            _itineraryStartLog.Add(log);
        }

        public void AddTripLog(string previousStation, string newStation, string previousTimeStamp, string currentTimeStamp)
        {
            string log = previousStation + "\t->\t" + newStation + " \t" + previousTimeStamp + "\t" + currentTimeStamp;
            _tourTripLog.Add(log);
        }

        public void AddRefuelLog(int refuelTime)
        {
            string log = "*** refuel in " + Convert.ToString(refuelTime) + " minutes ***";
            _tourTripLog.Add(log);
        }
        

    }
}
