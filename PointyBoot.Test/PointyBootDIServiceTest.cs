using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointyBoot.Base;
using PointyBootTest.TestData.Set1.C;
using System;
using System.Diagnostics;

namespace PointyBootTest
{
    [TestClass]
    public class PointyBootDIServiceTest
    {
        [DataTestMethod]
        [DataRow(1000)]
        public void TestInstantiationOfMultipleAutowiredClass(int count)
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
    }
}
