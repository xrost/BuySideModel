using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Gateway.Model;
using Gateway.Simulator.Events;

namespace Gateway.Simulator.ViewModel
{
	public class BrokerViewModel : ObservableObject
	{
		private readonly SellSide sellSide;
		private readonly Broker broker;

		internal BrokerViewModel(SellSide sellSide, Broker broker)
		{
			this.sellSide = sellSide;
			this.broker = broker;
			BrokerName = "Broker #" + broker.Id;
		}

		public void UpdateState()
		{
			RaisePropertyChanged(() => AllowedActions);
			RaisePropertyChanged(() => State);
			Messenger.Default.Send(new BrokerStateChangedEvent(Id, GetAllowedActions().ToArray()), Id);
		}

		public int Id => broker.Id;
		public string BrokerName { get; }

		public string AllowedActions
		{
			get
			{
				var actions = GetAllowedActions();
				return string.Join(" | ", actions);
			}
		}

		public string State
		{
			get
			{
				var result = GetState();
				if (broker.HasOrder && broker.Order.IsCancelRejected)
					result += " (CR)";
				return result;
			}
		}

		private string GetState()
		{
			if (broker.IsPending)
				return "Pending";

			if (broker.IsRejected)
				return "Rejected";

			if (broker.IsDeleted)
				return "Deleted";

			if (broker.Order.IsAllocated)
				return "Allocated";

			return "Order Confirmed";
		}

		private IEnumerable<BrokerAction> GetAllowedActions()
		{
			if (broker.IsPending)
			{
				yield return BrokerAction.Accept;
				yield return BrokerAction.Reject;
				yield break;
			}

			if (broker.HasOrder)
			{
				yield return BrokerAction.Allocate;
				yield return BrokerAction.Delete;
				if (sellSide.IsCancelled)
					yield return BrokerAction.RejectCancel;
			}
		}
	}
}