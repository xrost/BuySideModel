using System;

namespace Gateway.Model
{
	internal partial class SellSide
	{
		private class Cancelled : ISellSide
		{
			private readonly SellSide parent;

			public Cancelled(SellSide parent)
			{
				this.parent = parent;
			}

			private BrokerCollection Brokers => parent.brokers;

			public void Cancel()
			{
				throw new InvalidOperationException();
			}

			public void Accept(int brokerId)
			{
				Brokers[brokerId].Accept();
			}

			public void Reject(int brokerId)
			{
				Brokers[brokerId].RejectOrder();
				RaiseRejectedOrDeletedEvent();
			}

			public void Allocate(int brokerId, long allocation)
			{
				Brokers[brokerId].Order.Allocate(allocation);
			}

			public void Delete(int brokerId)
			{
				Brokers[brokerId].DeleteOrder();
				RaiseRejectedOrDeletedEvent();
			}

			public void RejectCancel(int brokerId)
			{
				Brokers[brokerId].Order.CancelRejected();
				RaiseRejectedOrDeletedEvent();
			}

			private void RaiseRejectedOrDeletedEvent()
			{
				var query = Brokers.Query();
				if (query.AllCancelRejected)
				{
					parent.Raise<SellSideEvents.CancelRejected>();
					parent.state = new Active(parent);
				}
				else if (query.AllRejected || query.AllInactive)
					parent.Raise<SellSideEvents.CancelConfirmed>();
			}
		}
	}
}