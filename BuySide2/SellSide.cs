using System;
using System.Collections.Generic;
using System.Linq;

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

		private void Raise<T>() where T: IDomainEvent
		{
			var evt = EventFactory.Create<T>(this);
			Raise(evt);
		}

		public int Id { get; }

		public void Cancel()
		{
			state.Cancel();
		}

		public void Accept(int brokerId)
		{
			state.Accept(brokerId);
		}

		public void Reject(int brokerId)
		{
			state.Reject(brokerId);
		}

		public void Allocate(int brokerId, long allocation)
		{
			state.Allocate(brokerId, allocation);
		}

		public void Delete(int brokerId)
		{
			state.Delete(brokerId);
		}

		public void RejectCancel(int brokerId)
		{
			state.RejectCancel(brokerId);
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


	static class BrokerLinq
	{
		public static IEnumerable<Broker> ExceptRejected(this IEnumerable<Broker> brokers) => brokers.Where(b => !b.IsRejected);
		public static IEnumerable<Broker> ExceptDeleted(this IEnumerable<Broker> brokers) => brokers.Where(b => !b.IsDeleted);
	}
}