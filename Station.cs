using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flying_Postman
{
    class Station
    {
        private string _name;
        private int _x;
        private int _y;

        public Station(string name, int x, int y)
        {
            _name = name;
            _x = x;
            _y = y;
            StationVisited = false;
        }

        public string StationName() { return _name; }
        public int StationX() { return _x; }
        public int StationY() { return _y; }
        public bool StationVisited { get; set; }

    }
}
