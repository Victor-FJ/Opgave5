using System;
using System.Collections.Generic;
using System.Text;
using Opgave1;

namespace Opgave5
{
    public static class BikeLibrary
    {
        private static List<Bike> _bikes = new List<Bike>()
        {
            new Bike(1, "Red", 1250, 3),
            new Bike(2, "White", 2545.50, 9),
            new Bike(3, "Blue&Black", 3000, 5)
        };


        public static List<Bike> Get()
        {
            if (_bikes != null && _bikes.Count != 0)
                return _bikes;
            return null;
        }

        public static Bike Get(int id)
        {
            Bike bike = GetBike(id);
            return bike;
        }

        public static int Post(Bike value)
        {
            for (int i = 1; i <= _bikes.Count + 1; i++)
                if (_bikes.Find(x => x.Id == i) == null)
                    value.Id = i;
            _bikes.Add(value);
            return value.Id;
        }


        private static Bike GetBike(int id)
        {
            return _bikes.Find(x => x.Id == id);
        }
    }
}
