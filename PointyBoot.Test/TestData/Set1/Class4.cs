using PointyBoot.Attributes.Provider;
using System.Diagnostics;

namespace PointyBootTest.TestData.Set1.D
{
    internal interface ISample
    {
        CoordA Prop1 { get; set; }
    }

    public class AutowiredClass : ISample
    {
        public CoordA Prop1 { get; set; }

        public void Test()
        {
            Debug.WriteLine($"Prop 1: \n{Prop1}");
        }
    }

    [PointyBindingProvider]
    public class Sample
    {

    }
}
