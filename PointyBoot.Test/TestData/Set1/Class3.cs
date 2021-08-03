﻿using PointyBoot.Attributes;
using PointyBoot.Attributes.Component;
using PointyBoot.Attributes.Provider;
using System.Diagnostics;

namespace PointyBootTest.TestData.Set1.C
{
    [PointyComponent]
    public class ComponentSample1
    {
        public ComponentSample1()
        {
        }

        public override string ToString()
        {
            return nameof(ComponentSample1);
        }
    }

    public class ComponentSample2
    {
        private readonly int x;

        public ComponentSample2(int x)
        {
            this.x = x;
        }

        public override string ToString()
        {
            return x.ToString();
        }
    }

    [PointyComponentFactory]
    public class ComponentProviderSample1
    {
        [PointyComponentProviderFunc]
        public ComponentSample1 ServeComponent()
        {
            return new ComponentSample1();
        }

        [PointyComponentProviderFunc(10001)]
        public ComponentSample2 ServeComponent(int x)
        {
            return new ComponentSample2(x);
        }
    }

    public class AutowiredClass
    {
        [Autowired]
        public CoordA Prop1 { get; set; }

        [Autowired]
        public Area Prop2 { get; set; }

        [Autowired]
        public ComponentSample2 Prop3 { get; set; }

        public void Test()
        {
            Debug.WriteLine($"Prop 1: \n{Prop1}");
            Debug.WriteLine($"Prop 2: \n{Prop2}");
            Debug.WriteLine($"Prop 3: \n{Prop3}");
        }
    }

    public class AutowiredSimpleClass1
    {
        [Autowired]
        public CoordA Prop1 { get; set; }

        [Autowired]
        public CoordB Prop2 { get; set; }

        public void Test()
        {
            Debug.WriteLine($"Prop 1: \n{Prop1}");
            Debug.WriteLine($"Prop 2: \n{Prop2}");
        }
    }

    public class AutowiredSimpleClass2
    {
        public CoordA Prop1 { get; set; }

        public CoordB Prop2 { get; set; }

        [Autowired]
        public AutowiredSimpleClass2(CoordA p1, CoordB p2)
        {
            Prop1 = p1;
            Prop2 = p2;
        }

        public void Test()
        {
            Debug.WriteLine($"Prop 1: \n{Prop1}");
            Debug.WriteLine($"Prop 2: \n{Prop2}");
        }
    }
}
