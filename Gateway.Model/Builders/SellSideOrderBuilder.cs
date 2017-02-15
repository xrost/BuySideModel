using System;

namespace Gateway.Model
{
	internal class SellSideOrderBuilder : ISellSideOrderState
	{
		private long? allocation;
		private bool cancelRejected;

		public SellSideOrderBuilder Allocated(long allocation)
		{
			this.allocation = allocation;
			return this;
		}


		public SellSideOrderBuilder CancelRejected()
		{
			cancelRejected = true;
			return this;
		}

		public static implicit operator SellSideOrder(SellSideOrderBuilder builder) => builder.Build();

		public SellSideOrder Build() => new SellSideOrder(this);

		public static SellSideOrderBuilder New() => new SellSideOrderBuilder();

		long? ISellSideOrderState.Allocation => allocation;

		bool ISellSideOrderState.CancelRejected => cancelRejected;
	}

	internal interface ISellSideOrderState
	{
		long? Allocation { get; }
		bool CancelRejected { get; }
	}
}