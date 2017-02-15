using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using GalaSoft.MvvmLight;
using Gateway.Model;
using Gateway.Simulator.Events;

namespace Gateway.Simulator.ViewModel
{
	public class GatewayViewModel : ViewModelBase
	{
		private SellSide sellSide;
		private EventDispatcher dispatcher;
		public ObservableCollection<BrokerViewModel> Brokers { get; } = new ObservableCollection<BrokerViewModel>();

		public GatewayViewModel()
		{
			if (IsInDesignModeStatic)
			{
				Brokers.Add(new BrokerViewModel(Create.SellSideWithId(1).Broker(b => b.WithId(1)), Create.BrokerWithId(1)));
				AddLog("Order Accepted");
			}
			MessengerInstance.Register<BuySideEvents.Completed>(this, OnBuySideCompleted);
			MessengerInstance.Register<BrokerActionEvent>(this, OnBrokerAction);
			MessengerInstance.Register<CancelOrderCommand>(this, CancelOrder);
		}

		public string Log { get; private set; } = string.Empty;

		private void AddLog(string message)
		{
			if (Log.Length > 0)
				Log += "\n" + message;
			else
				Log = message;
			RaisePropertyChanged(() => Log);
		}

		private void OnBuySideCompleted(BuySideEvents.Completed evt)
		{
			var builder =Create.SellSideWithId(evt.OrderId);
			foreach (var brokerId in evt.Brokers)
				builder.Broker(b => b.WithId(brokerId));
			sellSide = builder.Build();

			foreach (var broker in sellSide.Brokers)
			{
				var brokerViewModel = new BrokerViewModel(sellSide, broker);
				Brokers.Add(brokerViewModel);
			}
			dispatcher = new EventDispatcher(this);
			RaisePropertyChanged(() => State);
		}

		private void CancelOrder(CancelOrderCommand _)
		{
			sellSide.Cancel();
			AddLog("Buy side cancelled order");
			foreach (var broker in Brokers)
			{
				broker.UpdateState();
			}
			ProcessEvents();
		}

		private void OnBrokerAction(BrokerActionEvent evt)
		{
			try
			{
				PerformAction(evt);
				Brokers.First(b => b.Id == evt.BrokerId).UpdateState();
				ProcessEvents();
			}
			catch (InvalidOperationException)
			{
				MessageBox.Show($"Action {evt.Action} is not allowed");
			}
		}

		private void PerformAction(BrokerActionEvent evt)
		{
			switch (evt.Action)
			{
				case BrokerAction.Accept:
					sellSide.Accept(evt.BrokerId);
					break;
				case BrokerAction.Allocate:
					sellSide.Allocate(evt.BrokerId, 1);
					break;
				case BrokerAction.Delete:
					sellSide.Delete(evt.BrokerId);
					break;
				case BrokerAction.Reject:
					sellSide.Reject(evt.BrokerId);
					break;
				case BrokerAction.RejectCancel:
					sellSide.RejectCancel(evt.BrokerId);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(evt.Action), evt.Action.ToString());
			}
			AddLog($"Broker #{evt.BrokerId} {evt.Action}");
		}

		private void ProcessEvents()
		{
			dispatcher.Dispatch();
			RaisePropertyChanged(() => State);
		}

		public string State
		{
			get
			{
				if (sellSide == null)
					return "Waiting for BuySide";

				if (sellSide.IsCancelled)
					return "Cancelled";

				return "Active";
			}
		}

		class EventDispatcher : IDomainEventDispatcher
		{
			private readonly GatewayViewModel parent;
			private readonly MethodInfo genericMethod;

			public EventDispatcher(GatewayViewModel parent)
			{
				this.parent = parent;
				genericMethod = parent.MessengerInstance.GetType().GetMethods().First(m => m.Name == "Send" && m.GetParameters().Length == 1);
			}

			void IDomainEventDispatcher.Dispatch(IDomainEvent evt)
			{
				var method = genericMethod.MakeGenericMethod(evt.GetType());
				method.Invoke(parent.MessengerInstance, new object[] {evt});
			}

			public void Dispatch()
			{
				parent.sellSide.DispatchDomainEvents(this);
			}
		}
	}
}