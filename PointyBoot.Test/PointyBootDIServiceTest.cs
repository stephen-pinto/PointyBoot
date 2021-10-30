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
        [DataRow(1000000)]
        public void TestInstantiationOfMultipleAutowiredClass(int count)
        {
            PointyBootDIService service = new PointyBootDIService();
            ComponentProviderSample sample = new ComponentProviderSample();
            service.RegisterComponentFactory(sample);

            AutowiredClass[] array = new AutowiredClass[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = service.Get<AutowiredClass>();
            }
        }

        [DataTestMethod]
        [DataRow(1000000)]
        public void TestInstantiationOfMultipleClassNormally(int count)
        {
            
            AutowiredClass[] array = new AutowiredClass[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = new AutowiredClass();
                array[i].Prop1 = new TestData.Set1.CoordA();
                array[i].Prop2 = new TestData.Set1.Area(new TestData.Set1.CoordA(), new TestData.Set1.CoordB(), 1000);
                array[i].Prop3 = new ComponentSample2(1000);
            }
        }
    }
}
