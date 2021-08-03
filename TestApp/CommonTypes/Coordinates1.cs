using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.CommonTypes
{
	public class CoordA
	{
		public int X { get; set; }
		public int Y { get; set; }

		public CoordA()
		{
			X = 000;
			Y = 000;
		}

		public override string ToString()
		{
			return string.Format($"[A] X: {X} Y: {Y}");
		}
	}

	public class CoordB
	{
		public int X { get; set; }
		public int Y { get; set; }

		public CoordB()
		{
			X = 000;
			Y = 000;
		}

		public override string ToString()
		{
			return string.Format($"[B] X: {X} Y: {Y}");
		}
	}
}
