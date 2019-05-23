using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flying_Postman
{
    class Plane
    {
        private double _planeRange;
        private double _speed;
        private double _takeOffTime;
        private double _landingTime;
        private double _refuelTime;

        public Plane(double range, double speed, double takeOffTime, double landingTime, double refuelTime)
        {
            Range = range;
            _planeRange = range;
            _speed = speed;
            _takeOffTime = takeOffTime;
            _landingTime = landingTime;
            _refuelTime = refuelTime;
        }

        public double Range { get; set; }
        public double Speed { get { return _speed; } }
        public double TakeOffTime { get { return _takeOffTime; } }
        public double LandingTime { get { return _landingTime; } }
        public double RefuelTime { get { return _refuelTime; } }

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
