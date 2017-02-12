using System;
using System.Collections.Generic;

namespace Gateway.Model
{
	internal partial class SellSide : EventCollector
	{
		private readonly BrokerCollection brokers;
		private ISellSide state;

		public static SellSide CreateNew(int id)
		{
			var sellSide = new SellSide(SellSideBuilder.New(id));
			sellSide.Raise<BuySideEvents.Completed>();
			return sellSide;
		}

		public SellSide(ISellSideState state)
		{
			Id = state.Id;
			brokers = new BrokerCollection(state.Brokers);
			this.state = state.Cancelled ? (ISellSide) new Cancelled(this) : new Active(this);
		}

		public int Id { get; }

		public IEnumerable<Broker> Brokers => brokers;

		public bool IsCancelled => state is Cancelled;

		public void Cancel() => state.Cancel();

		public void Accept(int brokerId) => state.Accept(brokerId);

		public void Reject(int brokerId) => state.Reject(brokerId);

		public void Allocate(int brokerId, long allocation) => state.Allocate(brokerId, allocation);

		public void Delete(int brokerId) => state.Delete(brokerId);

		public void RejectCancel(int brokerId) => state.RejectCancel(brokerId);

		private void Raise<T>() where T: IDomainEvent
		{
			var evt = EventFactory.Create<T>(this);
			Raise(evt);
		}

		interface ISellSide
		{
			void Cancel();
			void Accept(int brokerId);
			void Reject(int brokerId);
			void Allocate(int brokerId, long allocation);
			void Delete(int brokerId);
			void RejectCancel(int brokerId);
		}
	}
}