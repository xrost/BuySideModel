using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Gateway.Model;
using Gateway.Simulator.Events;

namespace Gateway.Simulator.ViewModel
{
	public class BuySideViewModel : ViewModelBase
	{
		private readonly BuySide model = new BuySide(Create.BuySideWithId(1).ExpectedCount(2));

		public BuySideViewModel()
		{
			if (IsInDesignModeStatic)
			{
				model.Add("");
				model.Add("");
				Messages.Add("Order Accepted");
			}
			AddCommand = new RelayCommand(AddStep, () => !model.IsCompleted());
			CancelCommand = new RelayCommand(CancelOrder, () => model.IsCompleted());

			MessengerInstance.Register<SellSideEvents.Accepted>(this, _ => Messages.Add("Order Accepted"));
			MessengerInstance.Register<SellSideEvents.Rejected>(this, _ => Messages.Add("Order Rejected"));
			MessengerInstance.Register<SellSideEvents.Allocated>(this, _ => Messages.Add("Order Allocated"));
			MessengerInstance.Register<SellSideEvents.Cancelled>(this, _ => Messages.Add("Order Cancelled"));
			MessengerInstance.Register<SellSideEvents.CancelConfirmed>(this, _ => Messages.Add("Cancel Confirmed"));
			MessengerInstance.Register<SellSideEvents.CancelRejected>(this, _ => Messages.Add("Cancel Rejected"));
		}

		private void CancelOrder()
		{
			MessengerInstance.Send(new CancelOrderCommand());
		}

		private void AddStep()
		{
			model.Add("");
			RaisePropertyChanged(() => StepCount);
			if (model.IsCompleted())
				MessengerInstance.Send(new BuySideEvents.Completed(model.Id, new[] {1, 2, 3}));
		}

		public RelayCommand AddCommand { get; }
		public RelayCommand CancelCommand { get; }

		public int StepCount => model.Steps.Count;
		public int ExpectedStepCount => model.ExpectedStepCount;

		public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
	}
}
