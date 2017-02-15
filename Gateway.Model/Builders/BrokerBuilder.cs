using System;

namespace Gateway.Model
{
	internal class BrokerBuilder : IBrokerState
	{
		private int id;
		private DeactivateOrderAction action = DeactivateOrderAction.None;
		private SellSideOrder order;

		public BrokerBuilder(int id = 0)
		{
			this.id = id;
		}

		public BrokerBuilder WithId(int id)
		{
			this.id = id;
			return this;
		}

		public BrokerBuilder Rejected()
		{
			action = DeactivateOrderAction.Rejected;
			return this;
		}

		public BrokerBuilder Deleted()
		{
			action = DeactivateOrderAction.Deleted;
			return this;
		}

		public BrokerBuilder WithOrder(Action<SellSideOrderBuilder> build = null)
		{
			if (build == null)
			{
				order = new SellSideOrder();
			}
			else
			{
				var builder = SellSideOrderBuilder.New();
				build(builder);
				order = builder;
			}
			return this;
		}

		public static implicit operator Broker(BrokerBuilder builder) => builder.Build();

		public Broker Build() => new Broker(this);

		public static BrokerBuilder New(int id) => new BrokerBuilder(id);

		int IBrokerState.Id => id;
		DeactivateOrderAction IBrokerState.DeactivateAction => action;
		SellSideOrder IBrokerState.Order => order;
	}

	internal interface IBrokerState
	{
		int Id { get; }
		DeactivateOrderAction DeactivateAction { get; }
		SellSideOrder Order { get; }
	}
}