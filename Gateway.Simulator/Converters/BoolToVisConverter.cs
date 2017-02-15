using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gateway.Simulator.Converters
{
	public class BoolToVisConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var show = (bool) value;
			return show ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}