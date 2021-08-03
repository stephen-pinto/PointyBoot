using PointyBoot.Attributes;
using PointyBoot.Attributes.Component;
using PointyBoot.Attributes.Provider;
using PointyBoot.Core;
using System.Diagnostics;
using TestApp.CommonTypes;

namespace TestApp.Test2
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
    public class ComponentProviderSample
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

    public class Class2
    {
        public void Test()
        {
            PBServiceProvider manager = new PBServiceProvider();
            manager.RegisterComponentFactory<ComponentProviderSample>();
            var obj = manager.Get<AutowiredClass>();
            obj.Test();
        }
    }
}
