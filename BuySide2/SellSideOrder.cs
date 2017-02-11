using System;

namespace Gateway.Model
{
	internal class SellSideOrder
	{
		private long? allocation;

		public SellSideOrder()
		{
		}

		public SellSideOrder(ISellSideOrderState state)
		{
			allocation = state.Allocation;
			IsCancelRejected = state.CancelRejected;
		}

		public bool IsCancelRejected { get; private set; }
		public bool IsAllocated => allocation != null;

		public void Allocate(long? allocation)
		{
			this.allocation = allocation;
		}

		public void CancelRejected()
		{
			IsCancelRejected = true;
		}
	}
}