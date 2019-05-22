using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flying_Postman
{
    class Plane
    {
        private readonly int _planeRange;
        private readonly int _speed;
        private readonly int _takeOffTime;
        private readonly int _landingTime;
        private readonly int _refuelTime;

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
        public int GetSpeed() { return _speed; }
        public int GetTakeOffTime() { return _takeOffTime; }
        public int GetLandingTime() { return _landingTime; }
        public int GetRefuelTime() { return _refuelTime; }

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
