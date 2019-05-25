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
            if (args.Count() >= 3) // should have 3 or more command line arguments
            {
                try
                {
                    string planeFile = args[1];
                    Plane plane = ReadPlane(planeFile); // create plane object

                    string mailFile = args[0];
                    List<Station> stations = ReadStations(mailFile, plane); // create list of station objects

                    string initialTimeStamp = args[2]; // get initial time and convert to minutes;
                    int initialTimeMinutes = TourMath.ConvertToTimeMinutes(initialTimeStamp);

                    Tour tour = new Tour(stations, plane, initialTimeMinutes); // create and optimise a tour

                    Itinerary itinerary = new Itinerary(tour, plane); // create intinerary from tour

                    itinerary.PrintItinerary(); // print itinerary to console

                    // if flag given, try to output itinerary to output file
                    if (args[3] == "-o")
                    {
                        try { itinerary.SaveItinerary(args[4]); }
                        catch { Console.WriteLine("Error: Output file cannot be written to."); }
                    }
                    else { Console.WriteLine("Error: Please use '-o' to write itinerary to output file."); }
                }
                // handle errors accordingly:
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Error: Mail file or plane file cannot be read or does not exist.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Error: Inital time is incorrectly formatted. Please enter as HH:MM.");
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Error: The provided mail file or plane file cannot be parsed correctly.");
                }
            }
            else // if not, give simple message
            {
                Console.WriteLine("Error: Incorrect number of arguements provided.");
                Console.WriteLine("Please enter arguments in the format: mail_file plane_file HH:MM -o output_file");
                Console.WriteLine("'-o out_file' is optional.");
            }
            Console.ReadKey(); // wait to key press to finish program
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
            while (lineInMail != null) // while there are lines, create a station object
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
            // read plane file and create plane object

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
