using PointyBoot.Base;
using PointyBootTest.TestData.Set1.C;
using System;
using System.Diagnostics;
using System.Linq;

namespace TestApp
{
    class Program
    {
        private static void Test1()
        {
            PointyBootDIService service = new PointyBootDIService();
            ComponentProviderSample sample = new ComponentProviderSample();
            service.RegisterComponentFactory(sample);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            AutowiredClass[] array = new AutowiredClass[1000];
            for (int i = 0; i < 1000; i++)
            {
                array[0] = service.Get<AutowiredClass>();
            }

            var obj = array.First();
            obj.Print();

            sw.Stop();
            Console.WriteLine($"Completed allocations with PBServiceProvider in {sw.ElapsedMilliseconds} ms");
        }

        private static void Test2()
        {
            PointyBootDIService service = new PointyBootDIService();
            ComponentProviderSample sample = new ComponentProviderSample();
            service.RegisterComponentFactory(sample);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            AutowiredClass[] array = new AutowiredClass[1000];
            for (int i = 0; i < 1000; i++)
            {
                array[0] = service.Get<AutowiredClass>();
            }

            sw.Stop();
            Console.WriteLine($"Completed allocations with PBServiceProvider in {sw.ElapsedMilliseconds} ms");
        }

        static void Main(string[] args)
        {
            Test1();
            //Console.WriteLine("Hello World!");
        }
    }
}
