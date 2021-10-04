using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TestsEmailReciver
{
	class AddElementConverter : IValueConverter
	{
		public object NewElement { get; set; }


		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((IEnumerable<object>)value).Prepend(NewElement);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
