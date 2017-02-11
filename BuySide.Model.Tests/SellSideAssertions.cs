using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Gateway.Model.Tests
{
	internal static class SellSideAssertions
	{
		public static void AssertSingleEvent<T>(this SellSide sellSide) where T: IDomainEvent
		{
			var dispatcher = new TestEventDispatcher();
			sellSide.DispatchDomainEvents(dispatcher);
			Assert.Single(dispatcher);
			Assert.IsType<T>(dispatcher.First());
		}

		public static void AssertNoEvents(this SellSide sellSide)
		{
			var dispatcher = new TestEventDispatcher();
			sellSide.DispatchDomainEvents(dispatcher);
			Assert.Empty(dispatcher);
		}

		public static void AssertCancelledState(this SellSide sellSide)
		{
			Assert.Throws<InvalidOperationException>(() => sellSide.Cancel());
		}

		public static void AssertActiveState(this SellSide sellSide)
		{
			sellSide.Cancel();
		}
		

		class TestEventDispatcher : IDomainEventDispatcher, IEnumerable<IDomainEvent>
		{
			private readonly List<IDomainEvent> events = new List<IDomainEvent>();

			public void Dispatch(IDomainEvent @event)
			{
				events.Add(@event);
			}

			#region Enumerable

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public IEnumerator<IDomainEvent> GetEnumerator()
			{
				return events.GetEnumerator();
			}

			#endregion
		}
	}
}