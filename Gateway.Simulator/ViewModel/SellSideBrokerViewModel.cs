using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Gateway.Simulator.Events;

namespace Gateway.Simulator.ViewModel
{
	public class SellSideBrokerViewModel : ViewModelBase
	{
		private readonly int id;
		private readonly HashSet<BrokerAction> allowedActions = new HashSet<BrokerAction> {BrokerAction.Accept, BrokerAction.Reject};

		public SellSideBrokerViewModel(int id)
		{
			this.id = id;
			AcceptCommand = new RelayCommand(() => Raise(BrokerAction.Accept), () => allowedActions.Contains(BrokerAction.Accept));
			RejectCommand = new RelayCommand(() => Raise(BrokerAction.Reject), () => allowedActions.Contains(BrokerAction.Reject));
			AllocateCommand = new RelayCommand(() => Raise(BrokerAction.Allocate), () => allowedActions.Contains(BrokerAction.Allocate));
			DeleteCommand = new RelayCommand(() => Raise(BrokerAction.Delete), () => allowedActions.Contains(BrokerAction.Delete));
			RejectCancelCommand = new RelayCommand(() => Raise(BrokerAction.RejectCancel), () => allowedActions.Contains(BrokerAction.RejectCancel));

			MessengerInstance.Register<BrokerStateChangedEvent>(this, id, SetAllowedActions);
		}

		private void SetAllowedActions(BrokerStateChangedEvent evt)
		{
			allowedActions.Clear();
			allowedActions.UnionWith(evt.Actions);
		}

		public string Name => "Broker #" + id;
		public RelayCommand AcceptCommand { get; }
		public RelayCommand AllocateCommand { get; }
		public RelayCommand DeleteCommand { get; }
		public RelayCommand RejectCommand { get; }
		public RelayCommand RejectCancelCommand { get; }

		private void Raise(BrokerAction action)
		{
			Messenger.Default.Send(new BrokerActionEvent(id, action));
		}
	}

	public enum BrokerAction
	{
		Accept,
		Reject,
		Allocate,
		Delete,
		RejectCancel
	}
}