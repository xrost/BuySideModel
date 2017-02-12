using System;
using System.Collections.Generic;

namespace Gateway.Model.Api
{
	public class BuySideDto
	{
		public int Id;
		public int ExpectedCount;
		public List<object> Steps;
		public SellSideDto SellSide;
	}
}