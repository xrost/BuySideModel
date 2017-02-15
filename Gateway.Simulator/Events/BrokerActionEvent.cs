using System;
using Gateway.Simulator.ViewModel;

namespace Gateway.Simulator.Events
{
	public class BrokerActionEvent
	{
		public BrokerActionEvent(int brokerId, BrokerAction action)
		{
			BrokerId = brokerId;
			Action = action;
		}

		public int BrokerId { get; }
		public BrokerAction Action { get; }
	}
}