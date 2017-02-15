using System;

namespace Gateway.Model
{
	internal class BuySideBuilder : IBuySideState
	{
		private int id;
		private int count = 1;

		public BuySideBuilder(int id)
		{
			this.id = id;
		}

		public BuySideBuilder ExpectedCount(int count)
		{
			this.count = count;
			return this;
		}

		public static implicit operator BuySide(BuySideBuilder builder) => builder.Build();

		public BuySide Build() => new BuySide(this);

		public static BuySideBuilder New(int id) => new BuySideBuilder(id);

		int IBuySideState.Id => id;

		int IBuySideState.ExpectedCount => count;
	}

	interface IBuySideState
	{
		int Id { get; }
		int ExpectedCount { get; }
	}
}
