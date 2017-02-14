namespace Gateway.Model
{
	internal static class Create
	{
		public static BrokerBuilder BrokerWithId(int id) => new BrokerBuilder(id);
		public static SellSideBuilder SellSideWithId(int id) => SellSideBuilder.New(id);
		public static BuySideBuilder BuySideWithId(int id) => new BuySideBuilder(id);
	}
}