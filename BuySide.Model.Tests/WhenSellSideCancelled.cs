using System;
using Xunit;

namespace Gateway.Model.Tests
{
	public class WhenSellSideCancelled
	{
		private static SellSideBuilder CancelledSellSide() => Create.SellSideWithId(1).Cancelled();

		[Fact]
		public void When_Last_Broker_Rejected_CancelConfirmed_Raised()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1))
				.Broker(b => b.WithId(2).Rejected());

			sellSide.Reject(1);

			sellSide.AssertSingleEvent<SellSideEvents.CancelConfirmed>();
		}

		[Fact]
		public void When_Last_Broker_Deleted_CancelConfirmed_Raised()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1).WithOrder())
				.Broker(b => b.WithId(2).Deleted());

			sellSide.Delete(1);

			sellSide.AssertSingleEvent<SellSideEvents.CancelConfirmed>();
		}

		[Fact]
		public void One_Reject_One_Deletes_CancelConfirmed_Raised()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1))
				.Broker(b => b.WithId(2).Deleted());

			sellSide.Reject(1);

			sellSide.AssertSingleEvent<SellSideEvents.CancelConfirmed>();
		}

		[Fact]
		public void When_Last_Broker_RejectsCancel_CancelRejected_Raised()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1).WithOrder(o => o.CancelRejected()))
				.Broker(b => b.WithId(2).WithOrder());

			sellSide.RejectCancel(2);

			sellSide.AssertSingleEvent<SellSideEvents.CancelRejected>();
		}

		[Fact]
		public void When_One_Broker_Rejected_And_Other_RejectsCancel_CancelRejected_Raised()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1).Rejected())
				.Broker(b => b.WithId(2).WithOrder());

			sellSide.RejectCancel(2);

			sellSide.AssertSingleEvent<SellSideEvents.CancelRejected>();
		}

		[Fact]
		public void First_CancelReject_DoesNot_Raise_Event()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1).WithOrder())
				.Broker(b => b.WithId(2).WithOrder());

			sellSide.RejectCancel(2);

			sellSide.AssertNoEvents();
		}

		[Fact]
		public void When_CancelRejected_And_Other_Broker_Rejects_Order_CancelReject_Raised()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1).WithOrder(o => o.CancelRejected()))
				.Broker(b => b.WithId(2));

			sellSide.Reject(2);

			sellSide.AssertSingleEvent<SellSideEvents.CancelRejected>();
		}

		[Fact]
		public void It_Can_Not_Be_Cancelled_Again()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1))
				.Broker(b => b.WithId(2));

			Assert.Throws<InvalidOperationException>(() => sellSide.Cancel());
		}

		[Fact]
		public void One_Broker_CancelRejected_Other_Rejected_State_Becomes_Active()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1).WithOrder(o => o.CancelRejected()))
				.Broker(b => b.WithId(2));

			sellSide.Reject(2);

			sellSide.AssertActiveState();
		}

		[Fact]
		public void One_Broker_Rejected_Other_CancelRejected_State_Becomes_Active()
		{
			SellSide sellSide = CancelledSellSide()
				.Broker(b => b.WithId(1).Rejected())
				.Broker(b => b.WithId(2).WithOrder());

			sellSide.RejectCancel(2);

			sellSide.AssertActiveState();
		}
	}
}