using System;
using Xunit;

namespace Gateway.Model.Tests
{
	public class WhenSellSideActive
	{
		private static SellSideBuilder SellSide() => Create.SellSideWithId(1);

		[Fact]
		public void When_Last_Broker_Rejected_Sends_Rejected()
		{
			SellSide sellSide = SellSide()
				.Broker(b => b.WithId(1))
				.Broker(b => b.WithId(2).Rejected());

			sellSide.Reject(1);

			sellSide.AssertSingleEvent<SellSideEvents.Rejected>();
		}

		[Fact]
		public void When_First_Order_Accepted_AcceptedEvent_Raised()
		{
			SellSide sellSide = SellSide()
				.Broker(b => b.WithId(1))
				.Broker(b => b.WithId(2));

			sellSide.Accept(1);

			sellSide.AssertSingleEvent<SellSideEvents.Accepted>();
		}

		[Fact]
		public void When_First_Order_Accepted_FurtherAcceptance_DoNot_Raise()
		{
			SellSide sellSide = SellSide()
				.Broker(b => b.WithId(1))
				.Broker(b => b.WithId(2).WithOrder());

			sellSide.Accept(1);

			sellSide.AssertNoEvents();
		}

		[Fact]
		public void When_Cancelled_Event_Raised()
		{
			SellSide sellSide = SellSide()
				.Broker(b => b.WithId(1))
				.Broker(b => b.WithId(2));

			sellSide.Cancel();

			sellSide.AssertSingleEvent<BuySideEvents.Cancelled>();
		}

		[Fact]
		public void Second_Cancel_Not_Allowed()
		{
			SellSide sellSide = SellSide()
				.Broker(b => b.WithId(1))
				.Broker(b => b.WithId(2));

			sellSide.Cancel();

			Assert.Throws<InvalidOperationException>(() => sellSide.Cancel());
		}

		[Fact]
		public void When_Allocation_Exists_Cancel_Is_Not_Allowed()
		{
			SellSide sellSide = SellSide()
				.Broker(b => b.WithId(1).WithOrder(o => o.Allocated(10)))
				.Broker(b => b.WithId(2));

			Assert.Throws<InvalidOperationException>(() => sellSide.Cancel());
		}
    }
}
