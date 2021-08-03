using TestClass = TestApp.Test3.Class3;

namespace TestApp
{
    class Program
    {
        static void Main()
        {
            //PBContext dependencyManager = new PBContext();
            //var obj = dependencyManager.Get<Test1.Class1>();
            //obj.Test();

            TestClass obj = new TestClass();
            obj.Test();
            
            //OffTest test = new OffTest();
            //test.Test();
        }
    }
}
