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

            


            string planeFile = args[1];
            Plane plane = ReadPlane(planeFile);

            string mailFile = args[0];
            List<Station> stations = ReadStations(mailFile,plane);

            string initialTimeStamp = args[2];
            int initialTimeMinutes = TourMath.ConvertToTimeMinutes(initialTimeStamp);

            Tour tour = new Tour(stations, plane, initialTimeMinutes);

            Itinerary itinerary = new Itinerary(tour, plane);

            itinerary.PrintItinerary();

            Console.ReadKey();

        }

        static List<Station> ReadStations(string MAIL,Plane plane)
        {
            // read mail file and create station objects
            Console.WriteLine("Reading input from " + MAIL);

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
                Station station = new Station(name, x, y, plane);
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
            double range = Convert.ToDouble(PLANE_SPECS[0]);
            double speed = Convert.ToDouble(PLANE_SPECS[1]);
            double takeOffTime = Convert.ToDouble(PLANE_SPECS[2]);
            double landingTime = Convert.ToDouble(PLANE_SPECS[3]);
            double refuelTime = Convert.ToDouble(PLANE_SPECS[4]);

            Plane ultraLight = new Plane(range, speed, takeOffTime, landingTime, refuelTime);
            return ultraLight;
        }
        
    }
}
