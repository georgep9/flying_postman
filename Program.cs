using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Flying_Postman
{
    class Program
    {
        const char DELIM = ' ';

        static void Main(string[] args)
        {

            string mailFile = args[0];
            List<Station> stations = ReadStations(mailFile);


            string planeFile = args[1];
            Plane ultraLight = ReadPlane(planeFile);

            string initialTimeStamp = args[2];
            int initialTimeMinutes = TourMath.ConvertToTimeMinutes(initialTimeStamp);

            Tour tour = new Tour(stations, ultraLight, initialTimeMinutes);

            string outputArg = args[3];
            if (outputArg == "-o")
            {
                string fileName = args[4];
                Itinerary itinerary = tour.ReturnItinerary();
                itinerary.SaveItinerary(fileName);
            }

            Console.ReadKey();

        }

        static List<Station> ReadStations(string MAIL)
        {
            // read mail file and create station objects

            string lineInMail;
            string[] MAIL_ITEMS;

            FileStream inFileMail = new FileStream(MAIL, FileMode.Open, FileAccess.Read);
            StreamReader readerMail = new StreamReader(inFileMail);

            var Stations = new List<Station>();
            string name;
            int x, y;
            lineInMail = readerMail.ReadLine();
            while (lineInMail != null)
            {
                MAIL_ITEMS = lineInMail.Split(DELIM);
                name = MAIL_ITEMS[0];
                x = Convert.ToInt32(MAIL_ITEMS[1]);
                y = Convert.ToInt32(MAIL_ITEMS[2]);
                Station station = new Station(name, x, y);
                Stations.Add(station);
                lineInMail = readerMail.ReadLine();
            }

            return Stations;
        }

        static Plane ReadPlane(string PLANE)
        {
            // read boeing spec file and create plane object

            string lineInPlane;
            string[] PLANE_SPECS;

            FileStream inFilePlane = new FileStream(PLANE, FileMode.Open, FileAccess.Read);
            StreamReader readerPlane = new StreamReader(inFilePlane);

            lineInPlane = readerPlane.ReadLine();
            PLANE_SPECS = lineInPlane.Split(DELIM);
            int range = Convert.ToInt32(PLANE_SPECS[0]);
            int speed = Convert.ToInt32(PLANE_SPECS[1]);
            int takeOffTime = Convert.ToInt32(PLANE_SPECS[2]);
            int landingTime = Convert.ToInt32(PLANE_SPECS[3]);
            int refuelTime = Convert.ToInt32(PLANE_SPECS[4]);

            Plane ultraLight = new Plane(range, speed, takeOffTime, landingTime, refuelTime);
            return ultraLight;
        }
        
    }
}
