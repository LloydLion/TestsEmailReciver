using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestsEmailReciver
{
	public partial class AccountSettingsWindow : Window
	{
		internal AccountInfo NewAccount => (DataContext as AccountBuilder).Build();


		internal AccountSettingsWindow(AccountInfo baseAccount)
		{
			InitializeComponent();

			var builder = new AccountBuilder();
			if(baseAccount != null) builder.From(baseAccount);
			DataContext = builder;
		}


		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (DataContext == null) return;
			var box = sender as TextBox;
			(DataContext as AccountBuilder).Password = box.Text;
		}
	}
}
