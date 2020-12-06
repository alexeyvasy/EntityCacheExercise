using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EntityCacheExercise
{

    class Program : Observer
    {
        static void Main(string[] args)
        {
            // need to init empty folder : c:\temp\
            createCSVFile();

            Cache<Person> personCache = CacheFactory<Person>.GetCacheInstance(new CSVRepoProvider(@"c:\temp\persons.csv"), CacheMode.eager);
            Cache<Car> carCache = CacheFactory<Car>.GetCacheInstance(new CSVRepoProvider(@"c:\temp\cars.csv"), CacheMode.lazy);    // already have entities inside for testing purposes

            Program user = new Program();
            personCache.Subscribe(user);                // testing the Observer



            testAddGetEager(personCache);        // in lazy file there are already entries for testing purpose
            testUpdateGetEager(personCache);
            testRemoveGetEager(personCache);

            testGetLazy(carCache);
            testAddGetLazy(carCache);      
            testUpdateGetLazy(carCache);
            testRemoveGetLazy(carCache);

        }




        public void update(ActionType actionType, Entity entity)
        {
            Console.WriteLine("notifying user num:" + this.GetHashCode() + ". action of type " + actionType.ToString() + " was performed.\n");
        }

        private static void createCSVFile()
        {
            using (var writer = new StreamWriter(@"c:\temp\cars.csv", false))
            {
                writer.Write(@"111,Tesla,purple
222,mazda,red
333,Honda,yellow
");
            }
        }

        private static void testAddGetEager(Cache<Person> personCache)
        {
            Person alex = new Person(319618989, "Alexey", "Vasyanovich", Math.E);
            Person israel = new Person(123456789, "Israel", "Israeli", Math.PI);

            personCache.Add(alex);
            personCache.Add(israel);

            if (personCache.Get(alex.getId()).Equals(alex) && personCache.Get(israel.getId()).Equals(israel))
            {
                Console.WriteLine("Eager Add and Get test OK\n");
            }

        }
        private static void testUpdateGetEager(Cache<Person> personCache)
        {
            Person newalex = new Person(319618989, "Alex", "Vasy", 17);

            personCache.Update(newalex);

            if (personCache.Get(319618989).Equals(newalex))
            {
                Console.WriteLine("Eager Update and Get test OK\n");
            }
        }
        private static void testRemoveGetEager(Cache<Person> personCache)
        {
            personCache.Remove(319618989);

            if(personCache.Get(319618989) == null)
            {
                Console.WriteLine("Eager Remove and Get test OK\n");    
            }
        }
        

        private static void testAddGetLazy(Cache<Car> carCache)
        {
            Car honda = new Car(123, "honda", "white");

            carCache.Add(honda);
            Car h = carCache.Get(honda.getId());

            if (h.Equals(honda))
            {
                Console.WriteLine("Lazy Add and Get test OK\n");
            }
        }
        private static void testUpdateGetLazy(Cache<Car> carCache)
        {
            Car tesla = new Car(111, "Tesla", "purple");

            carCache.Update(tesla);

            if (carCache.Get(111).Equals(tesla))
            {
                Console.WriteLine("Lazy Update and Get test OK\n");
            }
        }
        private static void testRemoveGetLazy(Cache<Car> carCache)
        {
            carCache.Remove(123);

            if (carCache.Get(123) == null)
            {
                Console.WriteLine("Lazy Remove and Get test OK\n");
            }
        }
        private static void testGetLazy(Cache<Car> carCache)
        {
            Car c = new Car(111, "Tesla", "purple");

            carCache.Get(111);

            if (carCache.Get(111).Equals(c))
            {
                Console.WriteLine("Lazy Get test OK\n");
            }
        }
    }
}
