using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestsEmailReciver
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			DataContext = new MainWindowsViewModel(new TestRecord[]
			{
				new TestRecord("Ivanov Ivan 5", "12 У", "Some Test", 4, 67, 5) { PassDate = DateTime.Now.AddMinutes(-3), PassingTime = new TimeSpan(0, 12, 0), Url = "https://google.com" },
				new TestRecord("Ivanov Ivan 4", "13 У", "Some Test", 4, 67, 5) { PassDate = DateTime.Now.AddMinutes(-5), PassingTime = new TimeSpan(0, 12, 0), Url = "https://google.com" },
				new TestRecord("Ivanov Ivan 3", "14 У", "Some Test", 4, 67, 5) { PassingTime = new TimeSpan(0, 12, 0), Url = "https://yandex.ru" },
				new TestRecord("Ivanov Ivan 2", "15 У", "Some Test", 4, 67, 5) { PassingTime = new TimeSpan(0, 20, 0), Url = "https://google.com" },
				new TestRecord("Ivanov Ivan 1", "16 У", "Some Test", 4, 67, 5) { PassDate = DateTime.Now.AddMinutes(-3), PassingTime = new TimeSpan(0, 12, 0), Url = "https://google.com" }
			});
		}
	}
}
