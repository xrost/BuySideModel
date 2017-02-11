using System;
using Xunit;

namespace Gateway.Model.Tests
{
	public class BuySideTests
	{
		[Fact]
		public void SellSide_Created_When_BuySide_Completed()
		{
			BuySide buySide = BuySideBuilder.New(1).ExpectedCount(2);
			buySide.Add("");
			buySide.Add("");

			Assert.NotNull(buySide.SellSide);
		}

		[Fact]
		public void When_SellSideCreated_Event_Raised()
		{
			BuySide buySide = BuySideBuilder.New(1).ExpectedCount(2);
			buySide.Add("");
			buySide.Add("");

			buySide.SellSide.AssertSingleEvent<BuySideEvents.Completed>();
		}

		[Fact]
		public void CanNot_Add_More_Steps_Then_ExpectedCount()
		{
			BuySide buySide = BuySideBuilder.New(1).ExpectedCount(2);
			buySide.Add("");
			buySide.Add("");

			Assert.Throws<InvalidOperationException>(() => buySide.Add(""));
		}

		[Fact]
		public void CanNot_Cancel_Before_Completion()
		{
			BuySide buySide = BuySideBuilder.New(1).ExpectedCount(2);
			buySide.Add("");

			Assert.Throws<InvalidOperationException>(() => buySide.Cancel());
		}
	}
}