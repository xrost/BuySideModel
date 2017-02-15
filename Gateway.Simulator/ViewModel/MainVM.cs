using System;
using GalaSoft.MvvmLight;
using JetBrains.Annotations;

namespace Gateway.Simulator.ViewModel
{
	[UsedImplicitly]
	public class MainViewModel : ViewModelBase
	{
		public BuySideViewModel BuySide { get; } = new BuySideViewModel();
		public GatewayViewModel Gateway { get; } = new GatewayViewModel();
		public SellSideViewModel SellSide { get; } = new SellSideViewModel();

		public MainViewModel()
		{
		}


		public string StateName => "~";

		public override void Cleanup()
		{
			base.Cleanup();
			BuySide.Cleanup();
			Gateway.Cleanup();
			SellSide.Cleanup();
		}
	}
}