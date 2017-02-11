using System;
using System.Collections.Generic;

namespace Gateway.Model.Api
{
	public class SellSideDto
	{
		public int Id;
		public List<BrokerDto> Brokers;
		public bool IsCancelled;
	}

	public class BuySideDto
	{
		public int Id;
		public int ExpectedCount;
		public List<object> Steps;
		public SellSideDto SellSide;
	}
}