//using PointyBoot.Attributes;
//using PointyBoot.Attributes.Component;
//using PointyBoot.Attributes.Provider;
//using PointyBoot.Core;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using TestApp.CommonTypes;

//namespace TestApp.Test3
//{
//    [PointyComponent]
//    public class ComponentSample1
//    {
//        public ComponentSample1()
//        {
//        }

//        public override string ToString()
//        {
//            return nameof(ComponentSample1);
//        }
//    }

//    public class ComponentSample2
//    {
//        private readonly int x;

//        public ComponentSample2(int x)
//        {
//            this.x = x;
//        }

//        public override string ToString()
//        {
//            return x.ToString();
//        }
//    }

//    [PointyComponentFactory]
//    public class ComponentProviderSample
//    {
//        [PointyComponentProviderFunc]
//        public ComponentSample1 ServeComponent()
//        {
//            return new ComponentSample1();
//        }

//        [PointyComponentProviderFunc(10001)]
//        public ComponentSample2 ServeComponent(int x)
//        {
//            return new ComponentSample2(x);
//        }
//    }

//    public class AutowiredClass
//    {
//        [Autowired]
//        public CoordA Prop1 { get; set; }

//        [Autowired]
//        public Area Prop2 { get; set; }

//        [Autowired]
//        public ComponentSample2 Prop3 { get; set; }

//        public void Test()
//        {
//            Debug.WriteLine($"Prop 1: \n{Prop1}");
//            Debug.WriteLine($"Prop 2: \n{Prop2}");
//            Debug.WriteLine($"Prop 3: \n{Prop3}");
//        }
//    }

//    public class AutowiredSimpleClass
//    {
//        [Autowired]
//        public CoordA Prop1 { get; set; }

//        [Autowired]
//        public CoordB Prop2 { get; set; }       

//        public void Test()
//        {
//            Debug.WriteLine($"Prop 1: \n{Prop1}");
//            Debug.WriteLine($"Prop 2: \n{Prop2}");            
//        }
//    }

//    public class AutowiredSimple2Class
//    {
//        public CoordA Prop1 { get; set; }

//        public CoordB Prop2 { get; set; }

//        [Autowired]
//        public AutowiredSimple2Class(CoordA p1, CoordB p2)
//        {
//            Prop1 = p1;
//            Prop2 = p2;
//        }

//        public void Test()
//        {
//            Debug.WriteLine($"Prop 1: \n{Prop1}");
//            Debug.WriteLine($"Prop 2: \n{Prop2}");
//        }
//    }

//    class Class3
//    {
//        public void Test()
//        {
//            //Test2_Std();
//            Test2_Cust();

//            //Test1_Std();
//            //Test1_Cust();
//        }

//        public void Test1_Std()
//        {
//            var sw = new Stopwatch();
//            sw.Start();
//            ComponentProviderSample sample = new ComponentProviderSample();

//            var array = new AutowiredClass[1000];
//            for (int i = 0; i < 1000; i++)
//            {
//                array[0] = new AutowiredClass();
//                array[0].Prop1 = new CoordA();
//                array[0].Prop2 = new Area(new CoordA(), new CoordB(), 100);
//                array[0].Prop3 = sample.ServeComponent(10001);
//            }

//            sw.Stop();
//            Console.WriteLine($"Completed standard allocations in {sw.ElapsedMilliseconds} ms");           
//        }

//        public void Test1_Cust()
//        {
//            PBServiceProvider manager = new PBServiceProvider();
//            manager.RegisterComponentFactory<ComponentProviderSample>();

//            Stopwatch sw = new Stopwatch();
//            sw.Start();

//            AutowiredClass[] array = new AutowiredClass[1000];
//            for (int i = 0; i < 1000; i++)
//            {
//                array[0] = manager.Get<AutowiredClass>();
//            }

//            sw.Stop();
//            Console.WriteLine($"Completed allocations with PBServiceProvider in {sw.ElapsedMilliseconds} ms");
//        }

//        public void Test2_Std()
//        {
//            var sw = new Stopwatch();
//            sw.Start();
            
//            var array = new AutowiredSimpleClass[1000];
//            for (int i = 0; i < 1000; i++)
//            {
//                array[0] = new AutowiredSimpleClass();
//                array[0].Prop1 = new CoordA();
//                array[0].Prop2 = new CoordB();                
//            }

//            sw.Stop();
//            Console.WriteLine($"Completed standard allocations in {sw.ElapsedMilliseconds} ms");
//            sw.Restart();

//            var array2 = new AutowiredSimple2Class[1000];
//            for (int i = 0; i < 1000; i++)
//            {
//                array2[0] = new AutowiredSimple2Class(new CoordA(), new CoordB());
//            }

//            sw.Stop();
//            Console.WriteLine($"Completed standard allocations in {sw.ElapsedMilliseconds} ms");
//        }

//        public void Test2_Cust()
//        {
//            PBServiceProvider manager = new PBServiceProvider();
//            manager.RegisterComponentFactory<ComponentProviderSample>();

//            Stopwatch sw = new Stopwatch();
//            sw.Start();

//            AutowiredSimpleClass[] array = new AutowiredSimpleClass[1000];
//            for (int i = 0; i < 1000; i++)
//            {
//                array[0] = manager.Get<AutowiredSimpleClass>();
//            }

//            sw.Stop();
//            Console.WriteLine($"Completed allocations with PBServiceProvider in {sw.ElapsedMilliseconds} ms");
//            sw.Restart();
            
//            AutowiredSimple2Class[] array2 = new AutowiredSimple2Class[1000];
//            for (int i = 0; i < 1000; i++)
//            {
//                array2[0] = manager.Get<AutowiredSimple2Class>();
//            }

//            sw.Stop();
//            Console.WriteLine($"Completed allocations with PBServiceProvider in {sw.ElapsedMilliseconds} ms");
//        }
//    }
//}
