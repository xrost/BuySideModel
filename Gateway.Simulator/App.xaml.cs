using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Gateway.Simulator
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			this.DispatcherUnhandledException += OnDispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
		}

		private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			string message = "Unknown error";
			var ex = args.ExceptionObject as Exception;
			if (ex != null)
				message = ex.Message;
			MessageBox.Show("Error: " + message);
		}

		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
		{
			args.Handled = true;
			var ex = args.Exception;
			while (ex is TargetInvocationException)
				ex = ex.InnerException;
			MessageBox.Show("Error: " + ex.Message);
		}
	}
}
