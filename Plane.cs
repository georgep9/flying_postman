using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flying_Postman
{
    class Plane
    {
        private int _planeRange;
        private int _speed;
        private int _takeOffTime;
        private int _landingTime;
        private int _refuelTime;

        public Plane(int range, int speed, int takeOffTime, int landingTime, int refuelTime)
        {
            Range = range;
            _planeRange = range;
            _speed = speed;
            _takeOffTime = takeOffTime;
            _landingTime = landingTime;
            _refuelTime = refuelTime;
        }

        public double Range { get; set; }
        public int Speed { get { return _speed; } }
        public int TakeOffTime { get { return _takeOffTime; } }
        public int LandingTime { get { return _landingTime; } }
        public int RefuelTime { get { return _refuelTime; } }

        public void DisplaySpecs()
        {
            Console.WriteLine("Range: {0} Speed: {1} Take-off-time: {2} Landing-time: {3} Refuel-time: {4}",
                Range, _speed, _takeOffTime, _landingTime, _refuelTime);
        }

        public void RefuelPlane()
        {
            Range = _planeRange;
        }

        public bool IsFuelled()
        {
            if (Range == _planeRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
