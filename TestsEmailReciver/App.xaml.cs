using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TestsEmailReciver.Parsers;

namespace TestsEmailReciver
{
	public partial class App : Application
	{
		public static object StartupDataContext { get; private set; }


		private void Application_Startup(object sender, StartupEventArgs e)
		{
			var reciver = new EmailReciver(new AccauntInfo("server", 993, "email", "password"));
			var composeParser = new TestRecordComposeParser(new OnlineTestPadParser());

			StartupDataContext = new MainWindowViewModel(reciver, composeParser);
		}
	}
}
