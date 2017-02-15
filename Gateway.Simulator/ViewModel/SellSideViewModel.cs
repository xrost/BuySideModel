using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Gateway.Model;

namespace Gateway.Simulator.ViewModel
{
	public class SellSideViewModel : ViewModelBase
	{
		public ObservableCollection<SellSideBrokerViewModel> Brokers { get; } = new ObservableCollection<SellSideBrokerViewModel>();
		public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

		public SellSideViewModel()
		{
			if (IsInDesignModeStatic)
			{
				Messages.Add("Buyside order created");
				for (int i = 0; i < 3; i++)
				{
					Brokers.Add(new SellSideBrokerViewModel(i + 1));
				}
			}
			MessengerInstance.Register<BuySideEvents.Completed>(this, OnBuySideCompleted);
			MessengerInstance.Register<BuySideEvents.Cancelled>(this, OnCancelled);
		}

		private void OnCancelled(BuySideEvents.Cancelled evt)
		{
			Messages.Add("BuySide Cancelled Orders");
		}

		private void OnBuySideCompleted(BuySideEvents.Completed evt)
		{
			foreach(var brokerId in evt.Brokers)
			{
				var vm = new SellSideBrokerViewModel(brokerId);
				Brokers.Add(vm);
			}
			Messages.Add("BuySide Order Completed");
		}
	}
}