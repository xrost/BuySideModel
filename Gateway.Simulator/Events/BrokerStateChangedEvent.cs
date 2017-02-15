using System;
using System.Collections.Generic;
using Gateway.Simulator.ViewModel;

namespace Gateway.Simulator.Events
{
	public class BrokerStateChangedEvent
	{
		public BrokerStateChangedEvent(int brokerId, IReadOnlyCollection<BrokerAction> actions)
		{
			BrokerId = brokerId;
			Actions = actions;
		}

		public int BrokerId { get; }
		public IReadOnlyCollection<BrokerAction> Actions { get; }
	}
}