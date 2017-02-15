using System;

namespace Gateway.Model
{
	internal class SellSideOrder
	{
		public SellSideOrder()
		{
		}

		public SellSideOrder(ISellSideOrderState state)
		{
			Allocation = state.Allocation;
			IsCancelRejected = state.CancelRejected;
		}

		public long? Allocation { get; private set; }

		public bool IsCancelRejected { get; private set; }
		public bool IsAllocated => Allocation != null;

		public void Allocate(long? allocation)
		{
			Allocation = allocation;
		}

		public void CancelRejected()
		{
			IsCancelRejected = true;
		}
	}
}