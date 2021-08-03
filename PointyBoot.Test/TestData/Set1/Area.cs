using PointyBoot.Attributes;

namespace PointyBootTest.TestData.Set1
{
    public class Area
	{
		CoordA x;
		CoordB y;
		int defaultSum;

		[Autowired]
		public Area(CoordA x, CoordB y, int defaultSum)
		{
			this.x = x;
			this.y = y;
			this.defaultSum = defaultSum;
		}

		public override string ToString()
		{
			return $"{x} --- {y} == {defaultSum}";
		}
	}
}
