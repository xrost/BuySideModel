using System;
using System.Collections.Generic;

namespace Gateway.Model.Api
{
	public class SellSideDto
	{
		public int Id;
		public bool IsCancelled;
		public List<BrokerDto> Brokers;
	}
}