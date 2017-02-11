using System;
using System.Collections.Generic;

namespace Gateway.Model
{
	internal abstract class EventCollector
	{
		private List<IDomainEvent> events;

		protected void Raise(IDomainEvent evt)
		{
			if (events == null)
				events = new List<IDomainEvent>();
			events.Add(evt);
		}

		public void DispatchDomainEvents(IDomainEventDispatcher dispatcher)
		{
			if (events == null || events.Count == 0)
				return;

			if (dispatcher == null)
				throw new ArgumentNullException(nameof(dispatcher));

			foreach (var domainEvent in events)
			{
				dispatcher.Dispatch(domainEvent);
			}
			events.Clear();
		}
	}

	public interface IDomainEventDispatcher
	{
		void Dispatch(IDomainEvent @event);
	}
}