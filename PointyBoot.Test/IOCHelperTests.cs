using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointyBoot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointyBootTest
{
    [TestClass]
    public class IOCHelperTests
    {
        [DataTestMethod]
        public void Test()
        {
            TestData.Set1.CoordA obj = new TestData.Set1.CoordA();
            var funExpr = IOCHelper.BuildPropertySetterFunction(obj.GetType(), obj.GetType().GetProperties());
            var func = funExpr.Compile();
            func.DynamicInvoke(obj, new object[] { 5, 100 });
        }

    }
}
