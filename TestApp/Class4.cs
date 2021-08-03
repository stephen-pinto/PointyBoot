using PointyBoot.Attributes;
using PointyBoot.Attributes.Component;
using PointyBoot.Attributes.Provider;
using PointyBoot.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TestApp.CommonTypes;


namespace TestApp.Test4
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

    class Class4
    {
        public void Test()
        {
            PBServiceProvider manager = new PBServiceProvider();
            var obj = new Autowired();
            manager.AddSingleton<ISample>(obj);
        }
    }
}
