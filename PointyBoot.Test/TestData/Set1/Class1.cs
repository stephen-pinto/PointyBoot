using PointyBoot.Attributes;
using PointyBootTest.TestData.Set1;
using System.Diagnostics;

namespace PointyBootTest.Set1.A
{
    public class Class
	{
		[Autowired]
		public CoordA Prop { get; set; }

		[Autowired]
		public Area Prop2 { get; set; }

		public void Test()
		{
			Debug.WriteLine($"Prop 1: \n{Prop}");
			Debug.WriteLine($"Prop 2: \n{Prop2}");
		}
	}
}
