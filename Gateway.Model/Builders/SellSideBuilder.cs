using System;
using System.Collections.Generic;

namespace Gateway.Model
{
	internal class SellSideBuilder : ISellSideState
	{
		private int id;
		private bool cancelled;
		private readonly List<Broker> brokers = new List<Broker>();

		private SellSideBuilder(int id)
		{
			this.id = id;
		}

		public SellSideBuilder Cancelled()
		{
			cancelled = true;
			return this;
		}

		public SellSideBuilder Broker(Action<BrokerBuilder> build)
		{
			var builder = new BrokerBuilder();
			build(builder);
			brokers.Add(builder);
			return this;
		}

		public static implicit operator SellSide(SellSideBuilder builder) => builder.Build();

		public SellSide Build() => new SellSide(this);

		public static SellSideBuilder New(int id) => new SellSideBuilder(id);

		int ISellSideState.Id => id;
		bool ISellSideState.Cancelled => cancelled;
		IEnumerable<Broker> ISellSideState.Brokers => brokers;
	}

	internal interface ISellSideState
	{
		int Id { get; }
		bool Cancelled { get; }
		IEnumerable<Broker> Brokers { get; }
	}
}