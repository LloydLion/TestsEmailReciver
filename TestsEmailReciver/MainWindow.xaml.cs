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
		public MainWindow()
		{
			InitializeComponent();

			//DataContext = new MainWindowViewModel(new TestRecord[]
			//{
			//	new TestRecord("Ivanov Ivan 5", "12 У", "Some Test", 4, 67, 5) { PassDate = DateTime.Now.AddMinutes(-3), PassingTime = new TimeSpan(0, 12, 0), Url = "https://google.com" },
			//	new TestRecord("Ivanov Ivan 4", "13 У", "Some Test", 4, 67, 5) { PassDate = DateTime.Now.AddMinutes(-5), PassingTime = new TimeSpan(0, 12, 0), Url = "https://google.com" },
			//	new TestRecord("Ivanov Ivan 3", "14 У", "Some Test", 4, 67, 5) { PassingTime = new TimeSpan(0, 12, 0), Url = "https://yandex.ru" },
			//	new TestRecord("Ivanov Ivan 2", "15 У", "Some Test", 4, 67, 5) { PassingTime = new TimeSpan(0, 20, 0), Url = "https://google.com" },
			//	new TestRecord("Ivanov Ivan 1", "16 У", "Some Test", 4, 67, 5) { PassDate = DateTime.Now.AddMinutes(-3), PassingTime = new TimeSpan(0, 12, 0), Url = "https://google.com" }
			//});

			var reciver = new EmailReciver(new AccauntInfo("**server**", 000, "**email**", "**password**"));
			reciver.Connect();
			var emailCol = reciver.GetInbox();

			var composeParser = new TestRecordComposeParser(new OnlineTestPadParser());

			DataContext = new MainWindowViewModel(emailCol.Select(s => (composeParser.Parse(s.From[0].Address, s.Html, out _, out bool canParse), canParse)).Where(s => s.canParse).Select(s => s.Item1).AsSimpleNotify());
		}


		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var generator = listView.ItemContainerGenerator;

			for (int i = 0; i < generator.Items.Count; i++)
			{
				var item = generator.ContainerFromIndex(i) as FrameworkElement;
				item.Width = listView.ActualWidth - 10;
			}
		}
	}
}
