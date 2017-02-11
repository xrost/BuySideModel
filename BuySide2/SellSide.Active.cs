using System;

namespace Gateway.Model
{
	internal partial class SellSide
	{
		private class Active : ISellSide
		{
			private readonly SellSide parent;

			public Active(SellSide parent)
			{
				this.parent = parent;
			}

			private BrokerCollection Brokers => parent.brokers;

			public void Cancel()
			{
				parent.state = new Cancelled(parent);
				parent.Raise<BuySideEvents.Cancelled>();
			}

			public void Accept(int brokerId)
			{
				var noAcceptedOrders = !Brokers.HasAcceptedOrders();
				Brokers[brokerId].Accept();
				if (noAcceptedOrders && Brokers.HasAcceptedOrders())
					parent.Raise<SellSideEvents.Accepted>();
			}

			public void Reject(int brokerId)
			{
				Brokers[brokerId].RejectOrder();
				RaiseRejectedOrDeletedEvent();
			}

			public void Allocate(int brokerId, long allocation)
			{
				var hadAllocation = Brokers.HasAllocation();
				Brokers[brokerId].Order.Allocate(allocation);
				if (!hadAllocation && Brokers.HasAllocation())
					parent.Raise<SellSideEvents.Allocated>();
			}

			public void Delete(int brokerId)
			{
				Brokers[brokerId].DeleteOrder();
				RaiseRejectedOrDeletedEvent();
			}

			public void RejectCancel(int brokerId)
			{
				throw new InvalidOperationException();
			}

			private void RaiseRejectedOrDeletedEvent()
			{
				var query = Brokers.Query();

				if (query.AllRejected)
					parent.Raise<SellSideEvents.Rejected>();
				else if (query.AllInactive)
						parent.Raise<SellSideEvents.Cancelled>();
			}
		}
	}
}