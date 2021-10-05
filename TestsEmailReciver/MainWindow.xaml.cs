using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestsEmailReciver.Parsers;

namespace TestsEmailReciver
{
	public partial class MainWindow : Window
	{
		private Filtrator<TestRecord>.FilterHandler? secondFilterHandler;
		private Filtrator<TestRecord>.FilterHandler? primaryFilterHandler;


		private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;


		public MainWindow()
		{
			InitializeComponent();

			(Application.Current as App).Account.NewAccountUsed += (s, a) =>
			{
				DataContext = (Application.Current as App).StartupDataContext;
				ViewModel.RefreshEmails();
			};

			DataContext = (Application.Current as App).StartupDataContext;
		}


		private void FilterTypeComboBox_Selected(object sender, SelectionChangedEventArgs e)
		{
			var box = (ComboBox)sender;
			secondFilterHandler?.DisableFilter();

			switch (box.SelectedIndex)
			{
				case 0:
					secondFilterHandler = null;
					break;
				case 1:
					secondFilterHandler = ViewModel.SetFilter("name", filterValueTextBox.Text);
					break;
				case 2:
					secondFilterHandler = ViewModel.SetFilter("class", filterValueTextBox.Text);
					break;
			}
		}
		
		private void TestsComboBox_Selected(object sender, SelectionChangedEventArgs e)
		{
			var box = (ComboBox)sender;
			primaryFilterHandler?.DisableFilter();

			if (box.SelectedIndex == 0) primaryFilterHandler = null;
			else
			{
				primaryFilterHandler = ViewModel.SetFilter("test", e.AddedItems[0].ToString());
			}
		}

		private void FilterValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var text = (TextBox)sender;
			secondFilterHandler?.DisableFilter();
			
			if(secondFilterHandler.HasValue)
			{
				ViewModel.SetFilter(secondFilterHandler.Value.UniqueKey, text.Text);
			}
		}
	}
}
