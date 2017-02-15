using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Gateway.Model
{
	public interface IDomainEvent
	{
	}

	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
	public static class SellSideEvents
	{
		public class Accepted : IDomainEvent
		{
			public Accepted(int orderId)
			{
				OrderId = orderId;
			}

			public int OrderId { get; }
		}

		public class Rejected : IDomainEvent
		{
			public Rejected(int orderId)
			{
				OrderId = orderId;
			}

			public int OrderId { get; }
		}

		public class Allocated : IDomainEvent
		{
			public Allocated(int orderId)
			{
				OrderId = orderId;
			}

			public int OrderId { get; }
		}

		public class Cancelled : IDomainEvent
		{
			public Cancelled(int orderId)
			{
				OrderId = orderId;
			}

			public int OrderId { get; }
		}

		public class CancelConfirmed : IDomainEvent
		{
			public CancelConfirmed(int orderId)
			{
				OrderId = orderId;
			}

			public int OrderId { get; }
		}

		public class CancelRejected : IDomainEvent
		{
			public CancelRejected(int orderId)
			{
				OrderId = orderId;
			}

			public int OrderId { get; }
		}
	}

	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
	public static class BuySideEvents
	{
		public class Completed : IDomainEvent
		{
			public Completed(int orderId, IEnumerable<int> brokers)
			{
				OrderId = orderId;
				Brokers = brokers.ToList();
			}

			public int OrderId { get; }

			public IReadOnlyCollection<int> Brokers { get; }
		}

		public class Cancelled : IDomainEvent
		{
			public Cancelled(int orderId)
			{
				OrderId = orderId;
			}

			public int OrderId { get; }
		}
	}
}