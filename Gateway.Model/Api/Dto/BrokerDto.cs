namespace Gateway.Model.Api
{
	public class BrokerDto
	{
		public int Id;
		public BrokerStateDto State;
		public long? Allocation;
		public bool CancelRejected;
	}

	public enum BrokerStateDto
	{
		Pending,
		Accepted,
		Rejected,
		Deleted
	}
}