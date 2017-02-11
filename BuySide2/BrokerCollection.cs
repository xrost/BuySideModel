using System;
using System.Collections.Generic;
using System.Linq;

namespace Gateway.Model
{
	internal class BrokerCollection
	{
		private readonly IReadOnlyCollection<Broker> brokers;

		public BrokerCollection(IEnumerable<Broker> brokers)
		{
			this.brokers = brokers.ToList();
		}

		public Broker this[int brokerId]
		{
			get
			{
				var broker = brokers.FirstOrDefault(b => b.Id == brokerId);
				if (broker == null)
					throw new Exception("Broker not found");
				return broker;
			}
		}

		public bool HasAcceptedOrders() => brokers.Any(b => b.HasOrder);

		public bool HasAllocation() => brokers.Any(b => b.Order != null && b.Order.IsAllocated);

		//public bool AllCancelRejected() => Query().AllCancelRejected;
		//public bool AllInactive() => Query().AllInactive;
		//public bool AllRejected() => Query().AllRejected;

		public QueryResult Query() => new QueryResult(brokers);

		public struct QueryResult
		{
			public QueryResult(IEnumerable<Broker> brokers) : this()
			{
				int cancelRejectedCount = 0;
				int rejectedCount = 0;
				int deletedCount = 0;
				int orderCount = 0;

				foreach (var broker in brokers)
				{
					orderCount++;
					if (broker.IsRejected)
						rejectedCount++;
					if (broker.IsDeleted)
						deletedCount++;
					if (broker.HasOrder && broker.Order.IsCancelRejected)
						cancelRejectedCount++;
				}
				AllRejected = rejectedCount == orderCount;
				AllInactive = rejectedCount + deletedCount == orderCount;
				AllCancelRejected = cancelRejectedCount > 0 && cancelRejectedCount + rejectedCount + deletedCount == orderCount;
			}

			public readonly bool AllRejected;
			public readonly bool AllInactive;
			public readonly bool AllCancelRejected;
		}
	}
}